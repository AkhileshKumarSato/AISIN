using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using IOCLAndroidApp.Models;
using Java.Lang;
using Exception = System.Exception;

namespace SatoScanningApp.Adapter
{
    public class FGPickingBackLocationAdapter : RecyclerView.Adapter
    {
        public List<BinLocation> lstItem;
        Context context;
        public FGPickingBackLocationAdapter(List<BinLocation> itemDetails, Context cont)
        {
            lstItem = itemDetails;
            context = cont;
        }
        public override int ItemCount
        {
            get { return lstItem.Count; }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            try
            {
                FgPickingBackLocationHolder vh = holder as FgPickingBackLocationHolder;
                vh.txtLocationCode.Text = lstItem[position].LocationCode;
                vh.txtBinBarcode.Text = lstItem[position].BinBarcode;
                vh.txtQty.Text = lstItem[position].ScanQty.ToString();
            }
            catch (System.Exception ex) { Toast.MakeText(context, ex.Message, ToastLength.Long).Show(); }
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            FgPickingBackLocationHolder vh = null;
            try
            {
                View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_fgPick_back_location, parent, false);
                vh = new FgPickingBackLocationHolder(itemView);
            }
            catch (Exception ex) { Toast.MakeText(context, ex.Message, ToastLength.Long).Show(); }
            return vh;
        }
    }

    public class FgPickingBackLocationHolder : RecyclerView.ViewHolder
    {
        public TextView txtLocationCode;
        public TextView txtBinBarcode;
        public TextView txtQty;
        public FgPickingBackLocationHolder(View itemview) : base(itemview)
        {
            try
            {
                txtLocationCode = itemview.FindViewById<TextView>(Resource.Id.txtLocationCode);
                txtBinBarcode = itemview.FindViewById<TextView>(Resource.Id.txtBinBarcode);
                txtQty = itemview.FindViewById<TextView>(Resource.Id.txtQty);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}