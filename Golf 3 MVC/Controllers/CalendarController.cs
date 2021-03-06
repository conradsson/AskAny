using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DHTMLX.Scheduler;
using DHTMLX.Common;
using DHTMLX.Scheduler.Data;
using Golf_3_MVC.Models;
using Microsoft.AspNet.Identity;
using System.Net.Mail;

namespace Golf_3_MVC.Controllers
{
    public class CalendarController : Controller
    {
        dsu3Entities ds = new dsu3Entities();
        double totalHcp = 0;
        double mHcp;
        public ActionResult AutoComplete()
        {
            return View();
        }
        /// <summary>
        /// Autocomplete f�r medlemmar.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult GetAutoCompleteData(string term)
        {
            var result = ds.medlemmars.Where(x => x.fornamn.Contains(term))
                .Select(s => new GolfareAutoComplete { value = s.fornamn, fornamn = s.fornamn + " " + s.efternamn + " " + s.golf_id })
                .Union(ds.medlemmars.Where(x => x.efternamn.Contains(term))
                .Select(s => new GolfareAutoComplete { value = s.efternamn, fornamn = s.fornamn + " " + s.efternamn + " " + s.golf_id })
                .Union(ds.medlemmars.Where(x => x.golf_id.Contains(term))
                .Select(s => new GolfareAutoComplete { value = s.golf_id, fornamn = s.fornamn + " " + s.efternamn + " " + s.golf_id }))).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Autocomplete f�r bokningar.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult GetAutoCompleteDataBokning(string term)
        {
            var result = ds.boknings.Where(x => x.start_date.ToString().Contains(term))
                .Select(s => new BokningarAutoComplete { value = s.start_date.ToString(), text = s.start_date + " " + s.text + " ID: " + s.id }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// L�gga till medbokare.
        /// </summary>
        /// <param name="actionValues"></param>
        /// <param name="medlemsId"></param>
        /// <param name="sokBokning"></param>
        /// <param name="sokBokning2"></param>
        /// <param name="gast"></param>
        /// <returns></returns>
        public ActionResult Create(FormCollection actionValues, string medlemsId, string sokBokning, string sokBokning2, string gast)
        {
            medbokare medbokare = new medbokare();
            List<medbokare> aktuellaMedbokare = new List<medbokare>();
            CalendarBookings model = new CalendarBookings();
            medlemmar aktuellMedlem = new medlemmar();
            List<medlemmar> allaMedlemmar = new List<medlemmar>();

            string id = actionValues["Bokningar"];
            model.aktuellaMedbokare = aktuellaMedbokare;
            allaMedlemmar = ds.medlemmars.ToList();

            if (Request.Form["laggtill"] != null)
            {
                string bokningsID = sokBokning.Split(' ').Last();
                string golfID = medlemsId.Split(' ').Last();
                aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == bokningsID).ToList();

                if (aktuellaMedbokare.Count >= 4) // KONTROLL OM BOKNINGEN INNEH�LLER 4 PERSONER ELLER FLER
                {
                    TempData["msg"] = "<script>alert('Det finns redan fyra golfare i denna bokning');</script>";
                }
                else
                {
                    foreach (medbokare mb in aktuellaMedbokare) // LOOPAR IGENOM ALLA I BOKNINGEN O H�MTAR HCP SAMT KONTROLL F�R DUBBELBOKNING
                    {
                        try
                        {
                            if (mb.gast == true)
                            {

                            }
                            else
                            {
                                medlemmar m = new medlemmar();
                                double hcp;

                                m = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1.Trim()).FirstOrDefault();
                                aktuellMedlem = allaMedlemmar.Where(x => x.golf_id == golfID).FirstOrDefault();

                                hcp = Convert.ToDouble(m.hcp);
                                mHcp = Convert.ToDouble(aktuellMedlem.hcp);

                                totalHcp += hcp;

                                if (aktuellMedlem == m)
                                {
                                    TempData["msg"] = "<script>alert('Denna person finns redan med i bokningen');</script>";
                                    goto Foo;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    totalHcp += mHcp;
                    // MAX 100 HANDIKAPP
                    if (totalHcp >= 100) 
                    {
                        TempData["msg"] = "<script>alert('Bokningen g�r ej att g�ra d� det totala handikappet �r �ver 100');</script>";
                    }
                    // OM ALLT OK; L�GGER TILL PERSON
                    else
                    {
                        try
                        {
                            bokning aktuellB;
                            aktuellB = ds.boknings.Where(x => x.id.ToString() == bokningsID).FirstOrDefault();

                            medlemmar m;
                            m = allaMedlemmar.Where(x => x.golf_id == golfID).FirstOrDefault();

                            medbokare.Id = 33;
                            medbokare.BokningsId = Convert.ToInt32(bokningsID);
                            medbokare.Medbokare1 = golfID;
                            aktuellB.text += ", K�n: " + m.kon + " Handikapp: " + m.hcp;

                            ds.medbokares.Add(medbokare);
                            ds.SaveChanges();

                            TempData["msg"] = "<script>alert('Spelaren �r nu tillagd');</script>";

                            m = allaMedlemmar.Where(x => x.golf_id == medbokare.Medbokare1.Trim()).FirstOrDefault();
                            string epost = m.epost;
                            SendEmail(epost, "Bokning", "Du har blivit tillagd p� en bokning!" + aktuellB.start_date + " - " + aktuellB.end_date);
                        }
                        catch
                        {
                            TempData["msg"] = "<script>alert('Du m�ste fylla i b�de tid och person!');</script>";
                        }
                    }
                }
            }
            //TAR BORT EN MEDBOKARE FR�N EN BOKNING
            else if (Request.Form["tabort"] != null) 
            {
                string bokningsID = sokBokning.Split(' ').Last();
                string golfID = medlemsId.Split(' ').Last();
                aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == bokningsID).ToList();
                bokning aktuellBok = ds.boknings.Where(x => x.id.ToString() == bokningsID).FirstOrDefault();

                if (bokningsID == "" || golfID == "")
                {
                    TempData["msg"] = "<script>alert('Du m�ste fylla i b�de tid och person!');</script>";
                }
                else
                {
                    medbokare = aktuellaMedbokare.Where(x => x.Medbokare1 == golfID).FirstOrDefault();

                    if (medbokare == null)
                    {
                        TempData["msg"] = "<script>alert('Denna person finns ej med i denna bokning');</script>";
                        goto Foo;
                    }

                    foreach (medbokare mb in aktuellaMedbokare)
                    {
                        if (mb.Medbokare1.Trim() == golfID)
                        {
                            ds.medbokares.Remove(mb);
                            ds.SaveChanges();
                        }
                    }

                    aktuellBok.text = "";
                    aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == bokningsID).ToList();

                    //UPPDATERAR HCP o K�N F�R BOKNINGEN
                    foreach (medbokare mb in aktuellaMedbokare)
                    {
                        if (mb.gast == true)
                        {
                            aktuellBok.text += ", G�st: " + mb.Medbokare1;
                        }
                        else
                        {
                            medlemmar aktuellMed = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1).FirstOrDefault();

                            aktuellBok.text += ", K�n: " + aktuellMed.kon + " Handikapp: " + aktuellMed.hcp;
                        }
                    }

                    TempData["msg"] = "<script>alert('Spelaren �r nu borttagen');</script>";
                    ds.SaveChanges();
                }
            }
            else if (Request.Form["laggtillGast"] != null)
            {
                string bokningsIDgast = sokBokning2.Split(' ').Last();
                aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == bokningsIDgast).ToList();
                bokning aktuellBok = ds.boknings.Where(x => x.id.ToString() == bokningsIDgast).FirstOrDefault();

                //KONTROLL OM BOKNINGEN INNEH�LLER 4 (inkl. huvudbokare) PERSONER ELLER FLER
                if (aktuellaMedbokare.Count >= 4)
                {
                    TempData["msg"] = "<script>alert('Det finns redan fyra golfare i denna bokning');</script>";
                    goto Foo;
                }
                foreach (medbokare mb in aktuellaMedbokare)
                {
                    if (User.IsInRole("User"))
                    {
                        if (mb.gast == true)
                        {
                            TempData["msg"] = "<script>alert('Det finns redan en g�st i denna bokning');</script>";
                            goto Foo;
                        }
                    }
                }
                try
                {
                    bokning aktuellbokning;
                    aktuellbokning = ds.boknings.Where(x => x.id.ToString() == bokningsIDgast).FirstOrDefault();

                    medbokare.Id = 33;
                    medbokare.BokningsId = Convert.ToInt32(bokningsIDgast);
                    medbokare.Medbokare1 = gast;
                    medbokare.gast = true;
                    aktuellbokning.text += aktuellbokning.golf_id;
                    ds.medbokares.Add(medbokare);
                    ds.SaveChanges();

                    aktuellBok.text = "";
                    aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == bokningsIDgast).ToList();

                    //UPPDATERAR HCP o K�N F�R BOKNINGEN
                    foreach (medbokare mb in aktuellaMedbokare)
                    {
                        if (mb.gast == true)
                        {
                            aktuellBok.text += ", G�st: " + mb.Medbokare1;
                        }
                        else
                        {
                            medlemmar aktuellMed = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1).FirstOrDefault();

                            aktuellBok.text += ", K�n: " + aktuellMed.kon + " Handikapp: " + aktuellMed.hcp;
                        }
                        ds.SaveChanges();
                    }

                    TempData["msg"] = "<script>alert('Spelaren �r nu tillagd');</script>";

                }
                catch
                {
                    TempData["msg"] = "<script>alert('Du m�ste fylla i b�de tid och person!');</script>";
                }

            }
            else if (Request.Form["tabortGast"] != null)
            {
                string bokningsIDgast = sokBokning2.Split(' ').Last();
                aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == bokningsIDgast).ToList();
                bokning aktuellBok = ds.boknings.Where(x => x.id.ToString() == bokningsIDgast).FirstOrDefault();

                if (bokningsIDgast == "" || gast == "")
                {
                    TempData["msg"] = "<script>alert('Du m�ste fylla i b�de tid och person!');</script>";
                }
                else
                {
                    medbokare aktuellgast = new medbokare();
                    aktuellgast = aktuellaMedbokare.Where(x => x.Medbokare1 == gast).FirstOrDefault();

                    if (aktuellgast == null)
                    {
                        TempData["msg"] = "<script>alert('Du m�ste fylla i b�de tid och v�lja en g�st som �r inlagd!');</script>";
                    }
                    else
                    {
                        foreach (medbokare mb in aktuellaMedbokare)
                        {
                            if (mb.Medbokare1 == gast)
                            {
                                ds.medbokares.Remove(mb);
                                ds.SaveChanges();
                            }
                        }

                        aktuellBok.text = "";
                        aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == bokningsIDgast).ToList();

                        foreach (medbokare mb in aktuellaMedbokare) // UPPDATERAR HCP o K�N F�R BOKNINGEN
                        {
                            if (mb.gast == true)
                            {
                                aktuellBok.text += ", G�st: " + mb.Medbokare1;
                            }
                            else
                            {
                                medlemmar aktuellMed = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1).FirstOrDefault();

                                aktuellBok.text += ", K�n: " + aktuellMed.kon + " Handikapp: " + aktuellMed.hcp;
                            }
                        }

                        TempData["msg"] = "<script>alert('Spelaren �r nu borttagen');</script>";
                        ds.SaveChanges();
                    }
                }
            }
            Foo:
            return RedirectToAction("index");
        }
        /// <summary>
        /// Medlem l�gger till medbokare (person till bokning)
        /// </summary>
        /// <param name="actionValues"></param>
        /// <param name="golfidstring"></param>
        /// <param name="checkbox"></param>
        /// <returns></returns>
        public ActionResult CreateMedlem(FormCollection actionValues, string golfidstring, IEnumerable<bool> checkbox, string sokBokningM)
        {
            medbokare medbokare = new medbokare();
            List<medbokare> aktuellaMedbokare = new List<medbokare>();
            CalendarBookings model = new CalendarBookings();
            medlemmar aktuellMedlem = new medlemmar();

            string id = actionValues["Bokningar"];
            aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == id).ToList();
            model.aktuellaMedbokare = aktuellaMedbokare;

            List<medlemmar> allaMedlemmar = new List<medlemmar>();
            allaMedlemmar = ds.medlemmars.ToList();

            if (Request.Form["laggtill"] != null)
            {
                if (checkbox != null && checkbox.Count() == 2)
                {
                    //KONTROLL OM BOKNINGEN INNEH�LLER 4 PERSONER ELLER FLER
                    if (aktuellaMedbokare.Count >= 4)
                    {
                        TempData["msg"] = "<script>alert('Det finns redan fyra golfare i denna bokning');</script>";
                        goto Foo;
                    }
                    if (golfidstring == "")
                    {
                        TempData["msg"] = "<script>alert('Du m�ste fylla i namnet p� g�sten.');</script>";
                        goto Foo;
                    }

                    foreach (medbokare mb in aktuellaMedbokare)
                    {
                        if (mb.gast == true)
                        {
                            TempData["msg"] = "<script>alert('Det finns redan en g�st i denna bokning');</script>";
                            goto Foo;
                        }
                    }
                    bokning aktuellBok;
                    aktuellBok = ds.boknings.Where(x => x.id.ToString() == id).FirstOrDefault();

                    medbokare.Id = 33;
                    medbokare.BokningsId = Convert.ToInt32(id);
                    medbokare.Medbokare1 = golfidstring;
                    medbokare.gast = true;
                    aktuellBok.text += ", G�st: " + golfidstring;
                    ds.medbokares.Add(medbokare);
                    ds.SaveChanges();

                    TempData["msg"] = "<script>alert('Spelaren �r nu tillagd');</script>";
                    goto Foo;
                }
                //KONTROLL OM BOKNINGEN INNEH�LLER 4 PERSONER ELLER FLER
                if (aktuellaMedbokare.Count >= 4)
                {
                    TempData["msg"] = "<script>alert('Det finns redan fyra golfare i denna bokning');</script>";
                }
                else
                {
                    //LOOPAR IGENOM ALLA I BOKNINGEN O H�MTAR HCP SAMT KONTROLL F�R DUBBELBOKNING
                    foreach (medbokare mb in aktuellaMedbokare) 
                    {
                        if (mb.gast == true)
                        {
                        }
                        else
                        {
                            medlemmar m = new medlemmar();
                            double hcp;

                            m = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1.Trim()).FirstOrDefault();
                            aktuellMedlem = allaMedlemmar.Where(x => x.golf_id == golfidstring).FirstOrDefault();

                            if (aktuellMedlem == null)
                            {
                                TempData["msg"] = "<script>alert('Golf-ID:et existerar ej.');</script>";
                                goto Foo;
                            }

                            hcp = Convert.ToDouble(m.hcp);
                            mHcp = Convert.ToDouble(aktuellMedlem.hcp);

                            if (aktuellMedlem == m)
                            {
                                TempData["msg"] = "<script>alert('Denna person finns redan med i bokningen');</script>";
                                goto Foo;
                            }
                            totalHcp += hcp;
                        }
                    }
                    totalHcp += mHcp;

                    //MAX 100 HANDIKAPP
                    if (totalHcp >= 100)
                    {
                        TempData["msg"] = "<script>alert('Bokningen g�r ej att g�ra d� det totala handikappet �r �ver 100');</script>";
                    }
                    //OM ALLT OK: L�GGER TILL PERSON
                    else
                    {
                        try
                        {
                            bokning aktuellBok;
                            aktuellBok = ds.boknings.Where(x => x.id.ToString() == id).FirstOrDefault();
                            medlemmar m;
                            m = allaMedlemmar.Where(x => x.golf_id == golfidstring).FirstOrDefault();

                            if (m == null)
                            {
                                TempData["msg"] = "<script>alert('Golf-ID:et existerar ej.');</script>";
                            }
                            else
                            {
                                medbokare.Id = 33;
                                medbokare.BokningsId = Convert.ToInt32(id);
                                medbokare.Medbokare1 = golfidstring;
                                aktuellBok.text += ", K�n: " + m.kon + " Handikapp: " + m.hcp;
                                ds.medbokares.Add(medbokare);
                                ds.SaveChanges();

                                m = allaMedlemmar.Where(x => x.golf_id == medbokare.Medbokare1.Trim()).FirstOrDefault();
                                string epost = m.epost;
                                SendEmail(epost, "Bokning", "Du har blivit tillagd p� en bokning!");


                                TempData["msg"] = "<script>alert('Spelaren �r nu tillagd');</script>";
                            }
                        }
                        catch
                        {
                            TempData["msg"] = "<script>alert('Du m�ste fylla i b�de tid och v�lja person!');</script>";
                        }
                    }
                }
            }
            //TAR BORT EN MEDBOKARE FR�N EN BOKNING
            else if (Request.Form["tabort"] != null)
            {
                aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == id).ToList();
                bokning aktuellBok = ds.boknings.Where(x => x.id.ToString() == id).FirstOrDefault();

                if (checkbox != null && checkbox.Count() == 2)
                {
                    medbokare aktuellgast = new medbokare();
                    aktuellgast = aktuellaMedbokare.Where(x => x.Medbokare1 == golfidstring).FirstOrDefault();

                    //KOLLAR OM PERSONEN EXISTERAR ELLER OM PERSONEN INTE �R G�ST
                    if (aktuellgast == null || aktuellgast.gast != true) 
                    {
                        TempData["msg"] = "<script>alert('Du m�ste fylla i b�de tid och v�lja en g�st som �r inlagd!');</script>";
                    }
                    else
                    {
                        foreach (medbokare mb in aktuellaMedbokare)
                        {
                            if (mb.Medbokare1 == golfidstring && aktuellgast.gast == true)
                            {
                                ds.medbokares.Remove(mb);
                                ds.SaveChanges();
                            }
                        }
                        aktuellBok.text = "";
                        aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == id).ToList();

                        //UPPDATERAR HCP o K�N F�R BOKNINGEN
                        foreach (medbokare mb in aktuellaMedbokare) 
                        {
                            if (mb.gast == true)
                            {
                                aktuellBok.text += ", G�st: " + mb.Medbokare1;
                            }
                            else
                            {
                                medlemmar aktuellMed = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1).FirstOrDefault();

                                aktuellBok.text += ", K�n: " + aktuellMed.kon + " Handikapp: " + aktuellMed.hcp;
                            }
                        }

                        ds.SaveChanges();
                        TempData["msg"] = "<script>alert('Spelaren �r nu borttagen');</script>";
                    }
                }
                else
                {
                    try
                    {
                        foreach (medbokare mb in aktuellaMedbokare)
                        {
                            if (mb.Medbokare1.Trim() == golfidstring)
                            {
                                ds.medbokares.Remove(mb);
                                ds.SaveChanges();
                            }
                        }

                        aktuellBok.text = "";
                        aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == id).ToList();

                        //UPPDATERAR HCP o K�N F�R BOKNINGEN
                        foreach (medbokare mb in aktuellaMedbokare)
                        {
                            if (mb.gast == true)
                            {
                                aktuellBok.text += ", G�st: " + mb.Medbokare1;
                            }
                            else
                            {
                                medlemmar aktuellMed = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1).FirstOrDefault();

                                aktuellBok.text += ", K�n: " + aktuellMed.kon + " Handikapp: " + aktuellMed.hcp;
                            }
                        }

                        TempData["msg"] = "<script>alert('Spelaren �r nu borttagen');</script>";
                        ds.SaveChanges();

                        medlemmar m;
                        m = allaMedlemmar.Where(x => x.golf_id == golfidstring).FirstOrDefault();
                        string epost = m.epost;
                        SendEmail(epost, "Avbokning", "Du har blivit avbokad!");
                    }
                    catch
                    {
                        TempData["msg"] = "<script>alert('Du m�ste fylla i b�de tid och v�lja ett golf-ID som existerar!');</script>";
                    }
                }

            }
            if (Request.Form["bokaInDig"] != null)
            {
                string bokningsID = sokBokningM.Split(' ').Last();
                aktuellaMedbokare = ds.medbokares.Where(x => x.BokningsId.ToString() == bokningsID).ToList();

                if (aktuellaMedbokare.Count >= 4) // KONTROLL OM BOKNINGEN INNEH�LLER 4 PERSONER ELLER FLER
                {
                    TempData["msg"] = "<script>alert('Det finns redan fyra golfare i denna bokning');</script>";
                }
                else
                {
                    foreach (medbokare mb in aktuellaMedbokare) // LOOPAR IGENOM ALLA I BOKNINGEN O H�MTAR HCP SAMT KONTROLL F�R DUBBELBOKNING
                    {
                        if (mb.gast == true)
                        {
                        }
                        else
                        {
                            medlemmar m = new medlemmar();
                            double hcp;

                            m = allaMedlemmar.Where(x => x.golf_id == mb.Medbokare1.Trim()).FirstOrDefault();
                            aktuellMedlem = allaMedlemmar.Where(x => x.golf_id == User.Identity.GetUserName()).FirstOrDefault();

                            if (aktuellMedlem == null)
                            {
                                TempData["msg"] = "<script>alert('Golf-ID:et existerar ej.');</script>";
                                goto Foo;
                            }

                            hcp = Convert.ToDouble(m.hcp);
                            mHcp = Convert.ToDouble(aktuellMedlem.hcp);

                            if (aktuellMedlem == m)
                            {
                                TempData["msg"] = "<script>alert('Du finns redan med i bokningen');</script>";
                                goto Foo;
                            }
                            totalHcp += hcp;
                        }
                    }
                    totalHcp += mHcp;

                    if (totalHcp >= 100) // MAX 100 HANDIKAPP
                    {
                        TempData["msg"] = "<script>alert('Bokningen g�r ej att g�ra d� det totala handikappet �r �ver 100');</script>";
                    }
                    else // OM ALLT OK; L�GGER TILL PERSON
                    {
                        try
                        {
                            bokning aktuellBok;
                            List<bokning> allaBokningar = ds.boknings.ToList();

                            aktuellBok = allaBokningar.Where(x => x.id == Convert.ToInt32(bokningsID)).FirstOrDefault();
                            medlemmar m;
                            m = allaMedlemmar.Where(x => x.golf_id == User.Identity.GetUserName()).FirstOrDefault();

                            if (m == null)
                            {
                                TempData["msg"] = "<script>alert('Golf-ID:et existerar ej.');</script>";
                            }
                            else
                            {
                                medbokare.Id = 33;
                                medbokare.BokningsId = Convert.ToInt32(bokningsID);
                                medbokare.Medbokare1 = User.Identity.GetUserName();
                                aktuellBok.text += ", K�n: " + m.kon + " Handikapp: " + m.hcp;
                                ds.medbokares.Add(medbokare);
                                ds.SaveChanges();

                                m = allaMedlemmar.Where(x => x.golf_id == medbokare.Medbokare1.Trim()).FirstOrDefault();
                                string epost = m.epost;
                                SendEmail(epost, "Bokning", "Du har blivit tillagd p� en bokning!");

                                TempData["msg"] = "<script>alert('Spelaren �r nu tillagd');</script>";
                            }
                        }
                        catch
                        {
                            TempData["msg"] = "<script>alert('Du m�ste fylla i b�de tid och v�lja person!');</script>";
                        }
                    }
                }
            }
                Foo:
            return RedirectToAction("index");
        }
        /// <summary>
        /// Laddning av sj�lva bokningsschemat.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            season season = new season();
            var data = ds.seasons.Where(x => x.id == 1).FirstOrDefault();

            //KOLLAR OM S�SONGEN �R AKTIV
            if (data.seasontoggle == false)  
            {
                return View("offseason");
            }
            else
            {
                AutoDeleteBokning();  //Tar bort icke incheckade bokningar

                List<medlemmar> allaMedlemmar = new List<medlemmar>();
                medlemmar aktuellMedlem = new medlemmar();
                List<bokning> allaBokningar = new List<bokning>();
                List<bokning> minaBokningar = new List<bokning>();
                List<bokning> allablocktimeBokningar = new List<bokning>();
                List<medbokare> allaMedbokare = new List<medbokare>();
                List<medbokare> aktuellaMedbokningar = new List<medbokare>();
                List<bokning> aktuellaBokningar = new List<bokning>();

                CalendarBookings model = new CalendarBookings();

                allaMedlemmar = ds.medlemmars.ToList();
                aktuellMedlem = allaMedlemmar.Where(x => x.golf_id == User.Identity.GetUserName()).FirstOrDefault();
                allaBokningar = ds.boknings.ToList();
                allablocktimeBokningar = ds.boknings.Where(x => x.blocktime == true).ToList();
                allaMedbokare = ds.medbokares.ToList();
                aktuellaMedbokningar = allaMedbokare.Where(x => x.Medbokare1.Trim() == User.Identity.GetUserName()).ToList();
                model.medbokareLista = ds.medbokares.ToList();

                model.medlemsLista = allaMedlemmar;

                foreach (medbokare mb in aktuellaMedbokningar)
                {
                    bokning ny;
                    ny = allaBokningar.Where(x => x.id == mb.BokningsId).FirstOrDefault();

                    if (ny != null)
                    {
                        aktuellaBokningar.Add(ny);
                    }
                }

                model.minaBokningar = aktuellaBokningar;
                model.allaBokningar = allaBokningar;
                model.allaBlocktimeBokningar = allablocktimeBokningar;

                var sched = new DHXScheduler(this);
                sched.Skin = DHXScheduler.Skins.Flat;
                sched.Localization.Set(SchedulerLocalization.Localizations.Swedish);
                sched.Config.first_hour = 7;
                sched.Config.last_hour = 18;
                sched.Config.time_step = 10;

                sched.Config.start_on_monday = true;
                sched.InitialView = "month";
                sched.EnableDynamicLoading(SchedulerDataLoader.DynamicalLoadingMode.Month);
                sched.Config.separate_short_events = true;
                sched.Config.hour_size_px = 100;

                //H�MTAR OCH URSKILJER BLOCKTIME-BOKNINGAR UR BOKNINGAR
                foreach (bokning b in allaBokningar)
                {
                    if (b.blocktime == true)
                    {
                        sched.TimeSpans.Add(new DHXBlockTime()   //BLOCKAR ALLA BLOCKTIME-BOKNINGAR
                        {
                            StartDate = b.start_date,
                            EndDate = b.end_date
                        });
                    }
                }

                sched.TimeSpans.Add(new DHXBlockTime()   //BLOCKAR ALLT INNAN NU
                {
                    StartDate = new DateTime(2000, 1, 1),
                    EndDate = DateTime.Now
                });

                //KOLLAR OM DEN INLOGGADE �R EN MEDLEM
                if (User.IsInRole("User"))
                {
                    sched.TimeSpans.Add(new DHXBlockTime()   //INGA BOKNINGAR SENARE �N 1 M�NAD FRAM I TIDEN
                    {
                        StartDate = DateTime.Now.AddMonths(+1),
                        EndDate = DateTime.Now.AddYears(+1),
                    });

                    foreach (bokning bok in aktuellaBokningar)
                    {
                        sched.TimeSpans.Add(new DHXBlockTime()   //BLOCKAR ATT EN MEDLEM KAN G�RA EN BOKNING SAMMA DAG
                        {
                            StartDate = bok.start_date.Date,
                            EndDate = bok.start_date
                        });
                        sched.TimeSpans.Add(new DHXBlockTime()   //BLOCKAR ATT EN MEDLEM KAN G�RA EN BOKNING SAMMA DAG
                        {
                            StartDate = bok.end_date,
                            EndDate = bok.start_date.Date.AddDays(1)
                        });

                    }
                }

                sched.LoadData = true;
                sched.EnableDataprocessor = true;

                model.sched = sched;
                return View(model);
            }

        }
        List<medlemmar> medlemmars = new List<medlemmar>();
        public JsonResult S�k(string Prefix)
        {
            medlemmars = ds.medlemmars.ToList();
            return Json(medlemmars, JsonRequestBehavior.AllowGet);
        }
        public JsonResult s�kMedbokare(int bokID)
        {
            List<medbokare> bokningar = new List<medbokare>();
            bokningar = ds.medbokares.Where(x => x.BokningsId == bokID).ToList();

            return Json(bokningar, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// St�nga/�ppna banan f�r s�song.
        /// </summary>
        /// <param name="responsables"></param>
        /// <param name="checkResp"></param>
        /// <returns></returns>
        public ActionResult Seasontoggle(string responsables, bool checkResp = false)
        {
            season season = new season();

            season.seasontoggle = checkResp;

            if (season.seasontoggle == true)
            {
                var data = ds.seasons.Where(x => x.id == 1).FirstOrDefault();
                data.seasontoggle = season.seasontoggle;
                ds.SaveChanges();
            }
            else
            {
                var data = ds.seasons.Where(x => x.id == 1).FirstOrDefault();
                data.seasontoggle = season.seasontoggle;
                ds.SaveChanges();
            }

            return RedirectToAction("index");
        }

        public ContentResult Data()
        {
            try
            {
                var details = ds.boknings.ToList();
                return new SchedulerAjaxData(details);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Sparar bokningar till databasen, ta bort bokningar, uppdatera bokningar.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actionValues"></param>
        /// <returns></returns>
        ///
        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            try
            {
                var changedEvent = (bokning)DHXEventsHelper.Bind(typeof(bokning), actionValues);

                switch (action.Type)
                {
                    case DataActionTypes.Insert:

                        var diff = changedEvent.end_date.TimeOfDay - changedEvent.start_date.TimeOfDay;

                        if (diff.TotalHours > 0.17) // om det �r mer �n 10min //BLOCKTIME
                        {
                            if (User.IsInRole("Personal") || User.IsInRole("Admin")) //ENDAST F�R PERSONAL OCH ADMIN
                            {
                                bokning EV = new bokning();
                                EV.id = changedEvent.id;
                                EV.start_date = changedEvent.start_date;
                                EV.end_date = changedEvent.end_date;
                                EV.text = " " + changedEvent.text;
                                EV.golf_id = User.Identity.GetUserName();
                                EV.blocktime = true;
                                EV.incheckad = true;
                                ds.boknings.Add(EV);
                                ds.SaveChanges();
                                BlocktimeDeleteBokning(EV.id, EV.start_date, EV.end_date);

                                List<medlemmar> allaMedlemmar = new List<medlemmar>();
                                allaMedlemmar = ds.medlemmars.ToList();

                                medlemmar m;

                                m = allaMedlemmar.Where(x => x.golf_id == EV.golf_id).FirstOrDefault();

                                string epost = m.epost;
                                SendEmail(epost, "Avbokning", "Du har blivit avbokad!" + changedEvent.start_date + "-" + changedEvent.end_date);
                            }
                            //OM MEDLEM BOKAR MER �N 10 MINUTER
                            else
                            {
                                TempData["msg"] = "<script>alert('Du kan bara boka 10 minuter');</script>";
                            }
                        }
                        //VANLIG BOKNING
                        else
                        {
                            List<medlemmar> allaMedlemmar = new List<medlemmar>();
                            allaMedlemmar = ds.medlemmars.ToList();
                            medlemmar m;
                            m = allaMedlemmar.Where(x => x.golf_id == User.Identity.GetUserName()).FirstOrDefault();

                            //ENDAST F�R PERSONAL OCH ADMIN
                            if (User.IsInRole("Personal") || User.IsInRole("Admin")) 
                            {
                                bokning EV = new bokning();
                                EV.id = changedEvent.id;
                                EV.start_date = changedEvent.start_date;
                                EV.end_date = changedEvent.end_date;
                                EV.text = EV.text;
                                EV.golf_id = User.Identity.GetUserName();
                                EV.blocktime = false;
                                EV.incheckad = false;
                                ds.boknings.Add(EV);
                                ds.SaveChanges();

                                string epost = m.epost;
                                SendEmail(epost, "Bokning", "Du har blivit bokad!" + changedEvent.start_date + "-" + changedEvent.end_date);
                            }

                            // L�GGER TILL EN BOKNING
                            else
                            {
                                List<bokning> allaBokningar = ds.boknings.ToList();

                                bokning EV = new bokning();
                                medbokare MB = new medbokare();
                                EV.id = changedEvent.id;
                                EV.start_date = changedEvent.start_date;
                                EV.end_date = changedEvent.end_date;
                                EV.text = " K�n: " + m.kon + " Handikapp: " + m.hcp;
                                EV.golf_id = User.Identity.GetUserName();
                                EV.blocktime = false;
                                EV.incheckad = false;
                                ds.boknings.Add(EV);
                                ds.SaveChanges();

                                //L�GGER TILL EN RAD I MEDBOKARE,  I TRY F�R ATT ID:T SKA BLI �TKOMLIGT
                                try
                                {
                                    MB.Id = 33;
                                    MB.Medbokare1 = User.Identity.GetUserName();
                                    MB.BokningsId = EV.id;
                                    ds.medbokares.Add(MB);
                                    ds.SaveChanges();
                                }
                                catch (Exception)
                                {

                                    throw;
                                }

                                string epost = m.epost;
                                SendEmail(epost, "Bokning", "Du har blivit bokad!" + changedEvent.start_date + "-" + changedEvent.end_date);
                            }

                        }

                        break;
                    case DataActionTypes.Delete:

                        if (User.IsInRole("User") == true)
                        {
                            string golf_id = User.Identity.GetUserName();

                            foreach (var x in ds.medbokares)  // TAR BORT ALLA MEDBOKARE FR�N BOKNINGEN
                            {
                                if (x.BokningsId == id)
                                {
                                    ds.medbokares.Remove(x);
                                }
                            }
                            ds.SaveChanges();
                            var details = ds.boknings.Where(x => x.id == id && x.golf_id == golf_id).FirstOrDefault();

                            ds.boknings.Remove(details);
                            ds.SaveChanges();

                            List<medlemmar> allaMedlemmar = new List<medlemmar>();
                            allaMedlemmar = ds.medlemmars.ToList();

                            medlemmar m;

                            m = allaMedlemmar.Where(x => x.golf_id == User.Identity.GetUserName()).FirstOrDefault();

                            string epost = m.epost;
                            SendEmail(epost, "Avbokning", "Du har blivit avbokad!" + changedEvent.start_date + "-" + changedEvent.end_date);
                        }
                        else
                        {
                            foreach (var x in ds.medbokares)
                            {
                                if (x.BokningsId == id)
                                {
                                    ds.medbokares.Remove(x);
                                }
                            }
                            ds.SaveChanges();
                            var details = ds.boknings.Where(x => x.id == id).FirstOrDefault();

                            ds.boknings.Remove(details);
                            ds.SaveChanges();

                            List<medlemmar> allaMedlemmar = new List<medlemmar>();
                            allaMedlemmar = ds.medlemmars.ToList();

                            medlemmar m;

                            m = allaMedlemmar.Where(x => x.golf_id == User.Identity.GetUserName()).FirstOrDefault();

                            string epost = m.epost;
                            SendEmail(epost, "Avbokning", "Du har blivit avbokad!" + changedEvent.start_date + "-" + changedEvent.end_date);
                        }

                        break;
                    default://UPDATE
                        var data = ds.boknings.Where(x => x.id == id).FirstOrDefault();
                        data.start_date = changedEvent.start_date;
                        data.end_date = changedEvent.end_date;
                        data.text = " " + changedEvent.text;
                        ds.SaveChanges();
                        break;
                }
            }
            catch
            {
                action.Type = DataActionTypes.Error;
            }
            return (ContentResult)new AjaxSaveResponse(action);
        }
        /// <summary>
        /// Tar bort alla bokningar som finns i spannet d�r en blocktime l�gs in.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        public void BlocktimeDeleteBokning(int id, DateTime start, DateTime stop)
        {
            dsu3Entities ds3 = new dsu3Entities();
            bokning bok = new bokning();

            foreach (var i in ds.boknings)
            {
                if (i.start_date.TimeOfDay > start.TimeOfDay && i.end_date.TimeOfDay < stop.TimeOfDay && i.start_date.DayOfYear == start.DayOfYear)
                {
                    bok = ds3.boknings.Where(x => x.id == i.id).FirstOrDefault();
                    ds3.boknings.Remove(bok);
                    ds3.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Ta bort en blocktime om den ej �r aktuell l�ngre.
        /// </summary>
        /// <param name="actionValues"></param>
        /// <returns></returns>
        public ActionResult DeleteBlocktime(FormCollection actionValues)
        {
            dsu3Entities ds3 = new dsu3Entities();
            bokning bok = new bokning();
            string id = actionValues["BlocktimeBokningar"];

            if (Request.Form["tabort"] != null)
            {
                if (id == null)
                {
                    TempData["msg"] = "<script>alert('Du m�ste v�lja en bokning');</script>";
                    goto Boo;
                }

                bok = ds3.boknings.Where(x => x.id.ToString() == id).FirstOrDefault();

                if (bok.blocktime == true)
                {
                    ds3.boknings.Remove(bok);
                    ds3.SaveChanges();
                    TempData["msg"] = "<script>alert('Den blockerade tiden �r nu borttagen.');</script>";
                }
            }
            Boo:
            return RedirectToAction("index");
        }
        /// <summary>
        /// Incheckning av bokning.
        /// </summary>
        /// <param name="actionValues"></param>
        /// <returns></returns>
        public ActionResult Incheckning(FormCollection actionValues)
        {
            dsu3Entities ds3 = new dsu3Entities();
            bokning bok = new bokning();
            medbokare medbok = new medbokare();
            CalendarBookings score = new CalendarBookings();
            string id = actionValues["Bokningar"];
            string mid = actionValues["Checked1"];
            string mid1 = actionValues["Checked2"];
            string mid2 = actionValues["Checked3"];
            string mid3 = actionValues["Checked4"];

            if (Request.Form["checkainBtn"] != null)
            {
                if (id == null)
                {
                    TempData["msg"] = "<script>alert('Du m�ste v�lja en bokning');</script>";
                    goto Boo;
                }

                bok = ds3.boknings.Where(x => x.id.ToString() == id).FirstOrDefault();

                if (bok != null)
                {
                    if (mid != null)
                    {
                        medbok = ds3.medbokares.Where(x => x.Medbokare1.ToString() == mid).FirstOrDefault();
                        medbok.incheckad = true;
                        ds3.SaveChanges();
                    }
                    if (mid1 != null)
                    {
                        medbok = ds3.medbokares.Where(x => x.Medbokare1.ToString() == mid1).FirstOrDefault();
                        medbok.incheckad = true;
                        ds3.SaveChanges();
                    }
                    if (mid2 != null)
                    {
                        medbok = ds3.medbokares.Where(x => x.Medbokare1.ToString() == mid2).FirstOrDefault();
                        medbok.incheckad = true;
                        ds3.SaveChanges();
                    }
                    if (mid3 != null)
                    {
                        medbok = ds3.medbokares.Where(x => x.Medbokare1.ToString() == mid3).FirstOrDefault();
                        medbok.incheckad = true;
                        ds3.SaveChanges();
                    }
                    bok.incheckad = true;
                    ds3.SaveChanges();
                    TempData["msg"] = "Incheckningen lyckades.";
                }
            }
            Boo:
            return RedirectToAction("index");
        }
        /// <summary>
        /// Tar bort bokningnar som inte �r incheckade tio minuter innan dessa skall p�b�rjas.
        /// </summary>
        public void AutoDeleteBokning()
        {
            dsu3Entities ds3 = new dsu3Entities();
            bokning bok = new bokning();
            DateTime nuMinus10 = DateTime.Now.AddMinutes(10);

            foreach (var i in ds.boknings)
            {
                if (i.incheckad != true)
                {
                    if (nuMinus10 > i.start_date)
                    {
                        bok = ds3.boknings.Where(x => x.id == i.id).FirstOrDefault();

                        ds3.boknings.Remove(bok);
                        ds3.SaveChanges();

                    }
                }

            }
        }
        /// <summary>
        /// Metod f�r att skicka email, tar inparametrar som epost, subject och sj�lva bodyn.
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static void SendEmail(string toAddress, string subject, string body)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = body;


            var smtpClient = new SmtpClient { EnableSsl = true };
            smtpClient.Send(mailMessage);
        }
    }
}
