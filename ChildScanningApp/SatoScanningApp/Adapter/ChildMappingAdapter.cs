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
    public class ChildMappingAdapter : RecyclerView.Adapter
    {
        public List<PickChildPart> lstItem;
        Context context;
        public ChildMappingAdapter(List<PickChildPart> itemDetails, Context cont)
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
                DispatchHolder vh = holder as DispatchHolder;
                vh.txtBackNo.Text = lstItem[position].Kanban;

                vh.txtBackNo.SetBackgroundResource(Resource.Drawable.BorderStyle);


            }
            catch (System.Exception ex) { Toast.MakeText(context, ex.Message, ToastLength.Long).Show(); }
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            ChildMappingHolder vh = null;
            try
            {
                View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_ChildPart, parent, false);
                vh = new ChildMappingHolder(itemView);

            }
            catch (Exception ex) { Toast.MakeText(context, ex.Message, ToastLength.Long).Show(); }
            return vh;
        }
    }

    public class ChildMappingHolder : RecyclerView.ViewHolder
    {
        public TextView txtKanban;
        public ChildMappingHolder(View itemview) : base(itemview)
        {
            try
            {
                txtKanban = itemview.FindViewById<TextView>(Resource.Id.txtKanban);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}