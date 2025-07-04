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
    public class FGPickingAdapter : RecyclerView.Adapter
    {
        public List<PickList> lstItem;
        public event EventHandler<int> ItemClick;
        Context context;
        public FGPickingAdapter(List<PickList> itemDetails, Context cont)
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
                FgPickingHolder vh = holder as FgPickingHolder;
                vh.txtBackNo.Text = lstItem[position].BackNo;
                vh.txtQty.Text = lstItem[position].Qty.ToString();
                vh.txtScanQty.Text = lstItem[position].ScanQty.ToString();
                //Change Background color
                if (lstItem[position].ScanQty >= lstItem[position].Qty)
                {
                    vh.txtBackNo.SetBackgroundColor(Android.Graphics.Color.LightGreen);
                    vh.txtQty.SetBackgroundColor(Android.Graphics.Color.LightGreen);
                    vh.txtScanQty.SetBackgroundColor(Android.Graphics.Color.LightGreen);
                }
                else
                {
                    vh.txtBackNo.SetBackgroundResource(Resource.Drawable.BorderStyle);
                    vh.txtQty.SetBackgroundResource(Resource.Drawable.BorderStyle);
                    vh.txtScanQty.SetBackgroundResource(Resource.Drawable.BorderStyle);
                }
            }
            catch (System.Exception ex) { Toast.MakeText(context, ex.Message, ToastLength.Long).Show(); }
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            FgPickingHolder vh = null;
            try
            {
                View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_fgPicking, parent, false);
                vh = new FgPickingHolder(itemView, OnClick);

            }
            catch (Exception ex) { Toast.MakeText(context, ex.Message, ToastLength.Long).Show(); }
            return vh;
        }
        private void OnClick(int obj)
        {
            try
            {
                if (ItemClick != null)
                    ItemClick(this, obj);
            }
            catch (Exception ex) { Toast.MakeText(context, ex.Message, ToastLength.Long).Show(); }
        }
    }

    public class FgPickingHolder : RecyclerView.ViewHolder
    {
        public TextView txtBackNo;
        public TextView txtQty;
        public TextView txtScanQty;
        public ImageButton imgbtnViewLoc;
        public FgPickingHolder(View itemview, Action<int> listener) : base(itemview)
        {
            try
            {
                txtBackNo = itemview.FindViewById<TextView>(Resource.Id.txtBackNo);
                txtQty = itemview.FindViewById<TextView>(Resource.Id.txtQty);
                txtScanQty = itemview.FindViewById<TextView>(Resource.Id.txtScanQty);
                imgbtnViewLoc = itemview.FindViewById<ImageButton>(Resource.Id.imgbtnViewLoc);
                imgbtnViewLoc.Click += (sender, e) => listener(base.Position);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}