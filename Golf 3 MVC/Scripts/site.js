$(document).ready(function () {
    $(".navbar").hide(0).fadeIn(500)
    $("footer").hide(0).fadeIn(500)
    $(".body-content").hide(0).delay(450).fadeIn(1000)
    $(".carousel").hide(0).delay(450).fadeIn(1000)



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

    $(".panel-tavling").show();
    $(".pill-tavling").addClass("active")
});

$(".pill-tavling").click(function (e) {
    e.preventDefault()
    $(".panel-tavling").show();
    $(".pill-tavling").addClass("active")
});

//  BOKNINGSKALENDER FLIKEN-EFFEKTER
$(".kalender").ready(function (e) {
    $(".panel-kalender").show();
});



//$(window).scroll(function () {   // Return-to-top Funktion
//    if ($(this).scrollTop() >= 400) {
//        $('#return-to-top').fadeIn(200);
//    } else {
//        $('#return-to-top').fadeOut(200);
//    }
//});
//$('#return-to-top').click(function () {
//    $('body,html').animate({
//        scrollTop: 0
//    }, 300);
//});


//var scrollTo = function (identifier, speed) {  // Scroll-To function
//    $('html, body').animate({
//        scrollTop: $(identifier).offset().top
//    }, speed || 750);
//}





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

////

scheduler.form_blocks["color"] = {
    render: function (sns) {
        return "<div class='dhx_cal_block'><input type='color'/></div>";
    },
    set_value: function (node, value, ev) {
        node.firstChild.value = value || "";
    },
    get_value: function (node, ev) {
        return node.firstChild.value;
    },
    focus: function (node) {
        var a = node; scheduler._focus(a, true);
    }
}

/////

