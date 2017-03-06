$(document).ready(function () {
    $(".navbar").hide(0).fadeIn(500)
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

//  MEDLEM FLIKEN-EFFEKTER
$(".medlembtn").ready(function (e) {

    $(".panel-medlemmar").show();

});
//$(".medlembtn").click(function (e) {
//    $(".panel-medlemmar").show();
//});


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