using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ProtobufDemo.Model;
using ProtobufDemo.Ui.Adapter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtobufDemo.Data.EF.Manager;

namespace ProtobufDemo.Ui.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<IDataAdapter> adapters;
        private IDataAdapter selectedAdatper;
        private AdapterResult<IEnumerable<Order>> result;
        private bool isLoading;

        public MainViewModel()
        {
            this.InitAdapters();
            this.FetchDataCommand = new RelayCommand(this.LoadDataAsync, () => this.SelectedAdapter != null);
        }

        public bool IsLoading
        {
            get { return isLoading; }
            set { this.Set(ref isLoading, value); }
        }

        public ObservableCollection<IDataAdapter> Adapters
        {
            get { return adapters; }
            set { this.Set(ref adapters, value); }
        }

        public IDataAdapter SelectedAdapter
        {
            get { return selectedAdatper; }
            set
            {
                this.Set(ref selectedAdatper, value);
                this.FetchDataCommand.RaiseCanExecuteChanged();
            }
        }

        public AdapterResult<IEnumerable<Order>> Result
        {
            get { return result; }
            set { this.Set(ref result, value); }
        }

        public ObservableCollection<string> History { get; set; } = new ObservableCollection<string>();

        public RelayCommand FetchDataCommand { get; set; }

        private void InitAdapters()
        {
            var baseUrl = "http://*.azurewebsites.net";
            this.Adapters = new ObservableCollection<IDataAdapter>
            {
                new DataAdapter(new OrderManager(() => new Data.EF.DemoContext()), "direct EntityFramework Connection"),
                new DataAdapter(new Data.API.ApiOrderManager(baseUrl, Data.API.SerializationStrategy.JSON), "REST (JSON)"),
                new DataAdapter(new Data.API.ApiOrderManager(baseUrl, Data.API.SerializationStrategy.ProtoBuf), "REST (ProtoBuf)"),
            };
        }

        private async void LoadDataAsync()
        {
            this.IsLoading = true;

            this.Result = await this.SelectedAdapter.ReadDataAsync();
            this.History.Insert(0, $"{this.SelectedAdapter.Description} - Sent: {this.Result.BytesSent / 1024} KB, Received: {this.Result.BytesReceived / 1024} KB, Elapsed time: {this.Result.ElapsedMilliseconds}ms");

            this.IsLoading = false;
        }
    }
}
