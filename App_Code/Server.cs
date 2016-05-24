
using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using WebMatrix.Data;

public class Kisi
{
   
   private string Rumuz;
   
   public string Rumuz
   {
       get { return Rumuz; }
       set { Rumuz = value; }
   }


}

public class WebRtcHub3: Hub {
    public void Send(string message) {
       
        Clients.All.onMessageReceived(message);
    }
}

public class bulutDersChat : Hub
{
   
    public Task gorusmeyeKatil(string KanalKod,string Rumuz)
    {
   
        Groups.Add(Context.ConnectionId, KanalKod); 
        return Clients.Group(KanalKod).gorusmeyeKatildiIstemci(Rumuz,Context.ConnectionId);
       
    }

    public Task gorusmedenAyril(string KanalKod)
    {
        return Groups.Remove(Context.ConnectionId, KanalKod);
    }

    public void mesajGonder(string KanalKod, string Rumuz, string mesaj)
    {

        Clients.Group(KanalKod).mesajGonderIstemci(Rumuz, mesaj);
    }

    public override System.Threading.Tasks.Task OnDisconnected()
        {
            
            var db=Database.Open("fb1ae");
            var queryOnlineKullanici=db.QuerySingle("SELECT * FROM MesajlasmaKanaliOnlineKullanicilar WHERE ConnID=@0",Context.ConnectionId);
            Clients.Group(Convert.ToString(queryOnlineKullanici.KanalKod)).kullaniciCikti(Convert.ToString(Context.ConnectionId));
            db.Execute("DELETE FROM MesajlasmaKanaliOnlineKullanicilar WHERE ConnID=@0",Context.ConnectionId);
            db.Close();
           
            return base.OnDisconnected();
        }

}

