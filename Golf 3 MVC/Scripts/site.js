$(document).ready(function () {
    $(".navbar").fadeIn(500)
    $("footer").fadeIn(500)
    $(".body-content").delay(450).fadeIn(1000)
    $(".carousel").delay(450).fadeIn(1000)
});

//  START FLIKEN-EFFEKTER
$(".startbtn").ready(function (e) {
    $(".panel-nyheter").show();
    $(".panel-banstatus").hide();
    $(".pill-nyheter").addClass("active")
    $(".pill-banstatus").removeClass("active")
});

$(".pill-nyheter").click(function (e) {
    e.preventDefault()
    $(".panel-nyheter").show();
    $(".panel-banstatus").hide();
    $(".pill-nyheter").addClass("active")
    $(".pill-banstatus").removeClass("active")

});
$(".pill-banstatus").click(function (e) {
    e.preventDefault()
    $(".panel-nyheter").hide();
    $(".panel-banstatus").show();
    $(".pill-banstatus").addClass("active")
    $(".pill-nyheter").removeClass("active")
});

//  MEDLEM PERSONAL ADMIN FLIKEN-EFFEKTER
$(".medlembtn").ready(function (e) {

    $(".panel-medlemmar").show();

});

//  MEDLEM BESÖKARE FLIKEN-EFFEKTER
$(".medlembtn").ready(function (e) {

    $(".panel-infomedlem").show();
    $(".pill-infomedlem").addClass("active")

});
$(".pill-infomedlem").click(function (e) {
    e.preventDefault()
    $(".panel-infomedlem").show();
    $(".pill-infomedlem").addClass("active")
});



//  BANORNA FLIKEN-EFFEKTER
$(".banornabtn").ready(function (e) {

    $(".panel-bana1").show();
    $(".panel-bana2").hide();
    $(".panel-bana3").hide();
    $(".pill-bana1").addClass("active")
    $(".pill-bana2").removeClass("active")
    $(".pill-bana3").removeClass("active")
});

$(".pill-bana1").click(function (e) {
    e.preventDefault()
    $(".panel-bana1").show();
    $(".panel-bana2").hide();
    $(".panel-bana3").hide();
    $(".pill-bana1").addClass("active")
    $(".pill-bana2").removeClass("active")
    $(".pill-bana3").removeClass("active")
});
$(".pill-bana2").click(function (e) {
    e.preventDefault()
    $(".panel-bana2").show();
    $(".panel-bana1").hide();
    $(".panel-bana3").hide();
    $(".pill-bana2").addClass("active")
    $(".pill-bana1").removeClass("active")
    $(".pill-bana3").removeClass("active")
});
$(".pill-bana3").click(function (e) {
    e.preventDefault()
    $(".panel-bana3").show();
    $(".panel-bana1").hide();
    $(".panel-bana2").hide();
    $(".pill-bana3").addClass("active")
    $(".pill-bana1").removeClass("active")
    $(".pill-bana2").removeClass("active")
});

//  KLUBBEN FLIKEN-EFFEKTER
$(".klubbenbtn").ready(function (e) {

    $(".panel-omklubben").show();
    $(".pill-omklubben").addClass("active")
});

$(".pill-omklubben").click(function (e) {
    e.preventDefault()
    $(".panel-omklubben").show();
    $(".pill-omklubben").addClass("active")
});

//  KONTAKT FLIKEN-EFFEKTER
$(".kontaktbtn").ready(function (e) {

    $(".panel-kontaktuppgifter").show();
    $(".pill-kontaktuppgifter").addClass("active")
});

$(".pill-kontaktuppgifter").click(function (e) {
    e.preventDefault()
    $(".panel-kontaktuppgifter").show();
    $(".pill-kontaktuppgifter").addClass("active")
});

//  TÄVLING FLIKEN-EFFEKTER
$(".tavlingbtn").ready(function (e) {
    $(".panel-admintavling").hide();
    $(".panel-minaanmalningar").hide();
    $(".panel-tavling").show();
    $(".litavling").trigger('click');
    $(".pill-anmalan").addClass("active")
    $(".pill-admintavling").removeClass("active")
    $(".minaanmalningar").removeClass("active")
});

$("li.litavling").click(function (e) {
    e.preventDefault()
    $(".panel-tavling").show();
    $(".panel-minaanmalningar").hide();
    $(".panel-admintavling").hide();
    $(".pill-anmalan").addClass("active")
    $(".pill-admintavling").removeClass("active")
    $(".minaanmalningar").removeClass("active")
});

$(".pill-admintavling").click(function (e) {
    e.preventDefault()
    $(".panel-tavling").hide();
    $(".panel-minaanmalningar").hide();
    $(".panel-admintavling").show();
    $(".pill-admintavling").addClass("active")
    $(".pill-anmalan").removeClass("active")
    $(".minaanmalningar").removeClass("active")
});

$("li.minaanmalningar").click(function (e) {
    e.preventDefault()
    minaAnmalningar();
    $(".panel-tavling").hide();
    $(".panel-admintavling").hide();
    $(".panel-minaanmalningar").show();
    $(".pill-anmalan").addClass("active")
    $(".pill-admintavling").removeClass("active")
    $(".minaanmalningar").addClass("active")
});

//  BOKNINGSKALENDER FLIKEN-EFFEKTER
$(".kalender").ready(function (e) {
    $(".panel-kalender").show();
    $(".dhx_cal_tab_first").trigger('click');
});

// HÄMTAR MINA ANMÄLDA TÄVLINGAR
function minaAnmalningar() {
    $.ajax({
            url: '/tavlings/MinaTavlingar',
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            dataType: 'html',
            success: function (result) {
                $('.panel-minaanmalningar').html(result);
            }
        })
    }

// HÄMTAR AKTUELL TÄVLING UTIFRÅN TRYCK I DROPDOWN MENYN
function aktuelltavling(elem) {

    $(".panel-admintavling").hide();
    $(".panel-tavling").show();
    
    var id = $(elem).data('assigned-id');


    if (id != "") {

        $.ajax({
            url: '/tavlings/Aktuelltavling',
            contentType: 'application/html; charset=utf-8',
            data: { id },
            type: 'GET',
            dataType: 'html',
            success: function (result) {
                $('.panel-aktuelltavling').html(result);
            }
        })
    }
}


// START | Tonnys grejs | 10 minuters interval
var step = 10;
var format = scheduler.date.date_to_str("%H:%i");

scheduler.config.hour_size_px=(60/step)*22;
scheduler.templates.hour_scale = function(date){
    html="";
    for (var i=0; i<60/step; i++){
        html+="<div style='height:22px;line-height:22px;'>"+format(date)+"</div>";
        date = scheduler.date.add(date,step,"minute");
    }
    return html;
}

// SLUT | Tonnys grejs | 10 minuters interval
/////

var format = scheduler.date.date_to_str("%H:%i")
scheduler.xy.min_event_height = 41;
scheduler.templates.event_header = function(s,e,ev){
    return format(s) + " - " + format(s) + " " + ev.text;
}

////

scheduler.templates.event_body = function (s, e, ev) {
    return "";
}

/////

scheduler.renderEvent = function (container, ev) {
    var container_width = container.style.width; // e.g. "105px"

    // move section
    var html = "<div class='dhx_event_move my_event_move' style='width: " +
    container_width + "'></div>";

    // container for event's content
    html += "<div class='my_event_body'>";
    html += "<span class='event_date'>";
    //two options here:show only start date for short events or start+end for long
    if ((ev.end_date - ev.start_date) / 60000 > 40) {//if event is longer than 40 minutes
        html += scheduler.templates.event_header(ev.start_date, ev.end_date, ev);
        html += "</span><br/>";
    } else {
        html += scheduler.templates.event_date(ev.start_date) + "</span>";
    }
    // displaying event's text
    html += "<span>" + scheduler.templates.event_text(ev.start_date, ev.end_date, ev) +
    "</span>" + "</div>";

    // resize section
    html += "<div class='dhx_event_resize my_event_resize' style='width: " +
    container_width + "'></div>";

    container.innerHTML = html;
    return true; //required, true - display a custom form, false - the default form
};
///////




