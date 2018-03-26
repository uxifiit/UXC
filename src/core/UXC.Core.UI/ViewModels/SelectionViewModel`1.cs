using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.UI;

namespace UXC.Core.ViewModels
{
    public class SelectionViewModel<TItem> : BindableBase
    {
        public SelectionViewModel()
        {
            Items = new ObservableCollection<TItem>();
        }


        public SelectionViewModel(IEnumerable<TItem> items)
        {
            Items = new ObservableCollection<TItem>(items);
        }


        public ObservableCollection<TItem> Items { get; }


        public bool HasSelectedItem => IsIndexValid(selectedIndex, Items.Count);


        public TItem SelectedItem
        {
            get
            {
                return HasSelectedItem 
                     ? Items[selectedIndex] 
                     : default(TItem);
            }
            //set
            //{
            //    int index = Items.IndexOf(value);
            //    if (index == -1)
            //    {
            //        index = Items.Count;
            //        Items.Add(value);
            //    }
            //    SelectedIndex = index;
            //}
        }


        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                int index = IsIndexValid(value, Items.Count) ? value : -1;

                if (Set(ref selectedIndex, index))
                {
                    OnPropertyChanged(nameof(HasSelectedItem));
                    OnPropertyChanged(nameof(SelectedItem));

                    SelectedItemChanged?.Invoke(this, SelectedItem);
                }
            }
        }

        public event EventHandler<TItem> SelectedItemChanged;


        private static bool IsIndexValid(int index, int count)
        {
            return index >= 0 && index < count;
        }


        public bool TryGetSelectedItem(out TItem selectedItem)
        {
            if (HasSelectedItem)
            {
                selectedItem = SelectedItem;
                return true;
            }

            selectedItem = default(TItem);
            return false;
        }
    }
}
