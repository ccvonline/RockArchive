"use strict";$(document).on("campus.view.loaded",function(){$('[data-toggle="dropdown"]').on("click",function(){$(this).parent().toggleClass("open")})}),Handlebars.registerHelper("map",function(){var a="//maps.googleapis.com/maps/api/staticmap?zoom=12&size=300x300&markers=scale:2%7Cicon:http://bit.ly/1npXPPc%7C"+this.Location.Latitude+","+this.Location.Longitude+"&sensor=false&style=feature:all%7Csaturation:-100&scale=2",b='<div class="holder" style="background-size: 300px; background-image: url('+Handlebars.Utils.escapeExpression(a)+');"></div>';return new Handlebars.SafeString(b)}),Handlebars.registerHelper("toLowerCase",function(a){return(a||"").toLowerCase()});var campusSelected=!1,baseUrl="http://"+location.host+"/",selectCampus=function(a){$.ajax({type:"PUT",url:baseUrl+"api/Campuses/SetContext/"+a,error:function(a,b,c){console.log(b+" ["+c+"]: "+a.responseText)}}),campusSelected?($(".specific-campus").remove(),showCampusView(a)):($(".no-campus").remove(),showCampusView(a))},showCampusView=function(a){var b=$("#entry-template").html(),c=Handlebars.compile(b),d=CCV.findCampusById(a);d.otherCampuses=CCV.locations,$(".campus.section").append(c(d)),campusSelected=!0,$(document).trigger("campus.view.loaded")},showNoCampusView=function(){var a=$("#no-campus-template").html(),b=Handlebars.compile(a);$(".campus.section").append(b(CCV.locations)),$(document).trigger("campus.view.loaded")},loadCampuses=function(a){window.CCV=window.CCV||{},CCV.findCampusById=function(a){return CCV.locations.filter(function(b){return b.Id===parseInt(a)})[0]},$.ajax({url:baseUrl+"api/Campuses?$expand=Location&$select=Name,Id,ServiceTimes,Location",success:function(b){CCV.locations=b;for(var c=CCV.locations.length-1;c>=0;c--)if(CCV.locations[c].ServiceTimes){for(var d=CCV.locations[c].ServiceTimes.slice(0,-1),e=d.split("|"),f={saturday:[],sunday:[]},g=0;g<e.length;g++){var h=e[g].split("^");"Saturday"===h[0]&&f.saturday.push(h[1]),"Sunday"===h[0]&&f.sunday.push(h[1])}CCV.locations[c].ServiceTimes=f}a.call()},error:function(a,b,c){console.log(b+" ["+c+"]: "+a.responseText)}})};$(document).on("campus.view.loaded",function(){$(".js-select-campus-list a").each(function(){$(this).on("click",function(){var a=$(this),b=a.attr("data-campusid");return selectCampus(b),!1})})}),$(document).ready(function(){loadCampuses(showNoCampusView)}),$(document).ready(function(){$(".slide").fitVids()});var processSlidePosition=function(){$(".js-slide").each(function(){var a=$(this),b=a.position().top-$(window).scrollTop();0>=b?a.addClass("selected"):a.removeClass("selected"),0===b?a.addClass("current"):a.removeClass("current")})},scrollToSlide=function(a){$("html,body").animate({scrollTop:a.offset().top},300)},nextSlide=function(){processSlidePosition();var a=$(".js-slide").not(".selected").first();scrollToSlide(a.length>0?a:$(".js-slide").first())},prevSlide=function(){processSlidePosition();var a=$(".js-slide.selected").not(".current").last();scrollToSlide(a.length>0?a:$(".js-slide").last())};$(window).bind("scroll",function(){processSlidePosition()}),$(document).keydown(function(a){switch(a.which){case 37:prevSlide();break;case 38:prevSlide();break;case 39:nextSlide();break;case 40:nextSlide();break;default:return}a.preventDefault()}),$(document).ready(function(a){var b=a(".slide.one").height(),c=new ScrollMagic;new ScrollScene({duration:b}).setTween(TweenMax.to(".map.element",1,{y:-100,x:-50,ease:Linear.easeNone})).addTo(c),new ScrollScene({triggerElement:".slide.two",duration:b/3+b/2}).offset(-b/3).setTween(TweenMax.fromTo(".slide.two .centered.group",1,{opacity:0},{opacity:1,ease:Linear.easeNone})).addTo(c),new ScrollScene({triggerElement:".slide.three",duration:b/3+b/2}).offset(-b/3).setTween(TweenMax.fromTo(".slide.three .centered.group",1,{opacity:0},{opacity:1,ease:Linear.easeNone})).addTo(c),new ScrollScene({triggerElement:".slide.four",duration:b/3+b/2}).offset(-b/3).setTween(TweenMax.fromTo(".slide.four .centered.group",1,{opacity:0},{opacity:1,ease:Linear.easeNone})).addTo(c),new ScrollScene({triggerElement:"nav[role=main]"}).offset(-b/2+a("nav[role=main]").height()).setTween(TweenMax.from("nav[role=main] ul",1.5,{opacity:0,y:-20})).addTo(c)});