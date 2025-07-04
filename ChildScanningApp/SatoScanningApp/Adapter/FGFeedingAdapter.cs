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
    public class FGFeedingAdapter : RecyclerView.Adapter
    {
        public List<FgFeeding> lstItem;
        Context context;
        public FGFeedingAdapter(List<FgFeeding> itemDetails, Context cont)
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
                FgFeedingHolder vh = holder as FgFeedingHolder;
                vh.txtLocationCode.Text = lstItem[position].LocationCode;
                vh.txtCapacity.Text = lstItem[position].Capacity.ToString();
                vh.txtAvailableQty.Text = lstItem[position].AvailableQty.ToString();
            }
            catch (System.Exception ex) { Toast.MakeText(context, ex.Message, ToastLength.Long).Show(); }
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            FgFeedingHolder vh = null;
            try
            {
                View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_fgfeeding, parent, false);
                vh = new FgFeedingHolder(itemView);

            }
            catch (Exception ex) { Toast.MakeText(context, ex.Message, ToastLength.Long).Show(); }
            return vh;
        }
    }

    public class FgFeedingHolder : RecyclerView.ViewHolder
    {
        public TextView txtLocationCode;
        public TextView txtCapacity;
        public TextView txtAvailableQty;
        public FgFeedingHolder(View itemview) : base(itemview)
        {
            try
            {
                txtLocationCode = itemview.FindViewById<TextView>(Resource.Id.txtLocationCode);
                txtCapacity = itemview.FindViewById<TextView>(Resource.Id.txtCapacity);
                txtAvailableQty = itemview.FindViewById<TextView>(Resource.Id.txtAvailableQty);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}