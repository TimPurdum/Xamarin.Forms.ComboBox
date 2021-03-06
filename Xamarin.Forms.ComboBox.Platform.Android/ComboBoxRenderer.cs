﻿using Android.Content;
using Android.Widget;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.ComboBox;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(ComboBox), typeof(ComboBoxRenderer))]
namespace Xamarin.Forms.ComboBox
{
    public class ComboBoxRenderer : ViewRenderer<ComboBox, FormsSpinner>
    {
        #region Fields
        private bool _isDisposed;
        #endregion

        public ComboBoxRenderer(Context context)
            : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ComboBox> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var spinner = CreateNativeControl();

                    spinner.ItemSelected += Spinner_ItemSelected;
                    var items = e.NewElement.ItemsSource;

                    if (items == null)
                    {
                        spinner.Adapter = null;
                    }
                    else
                    {
                        // spinner.Adapter = new ArrayAdapter(Context, global::Android.Resource.Layout.SimpleSpinnerItem, items);
                        spinner.Adapter = new FormsSpinnerAdapter(Context, items, Element.TextColor.ToAndroid());
                    }

                    SetNativeControl(spinner);
                    Element.SelectedIndexChanged += Element_SelectedIndexChanged;
                    UpdateSelectedIndex();
                    UpdateTextColor();
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
            {
                UpdateSelectedIndex();
            }
            else if (e.PropertyName == Picker.TextColorProperty.PropertyName)
            {
                UpdateTextColor();
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private void Element_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateSelectedIndex();
        }

        private void UpdateSelectedIndex()
        {
            var index = Element.SelectedIndex;
            if (Control.SelectedItemPosition != index)
            {
                Control.SetSelection(index);
            }
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            UpdateSelectedItem();
        }

        private void UpdateSelectedItem()
        {
            if (Element.SelectedIndex != Control.SelectedItemPosition)
            {
                Element.SelectedIndex = Control.SelectedItemPosition;
            }
        }

        private void UpdateTextColor()
        {
            var adapter = Control.Adapter as FormsSpinnerAdapter;
            if (adapter != null)
            {
                adapter.TextColor = Element.TextColor.ToAndroid();
            }
        }

        protected override FormsSpinner CreateNativeControl()
        {
            // Base.Widget.AppCompat.Spinner.Underlined
           // , null, global::Android.Resource.Style.WidgetSpinnerDropDown
           var spinner = new FormsSpinner(Context)
            {
                Focusable = true,
                Clickable = true,
                Tag = this
            };
            return spinner;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                _isDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
