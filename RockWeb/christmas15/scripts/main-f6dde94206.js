"use strict";function findNearestCampus(e){function t(e,t){if(t===google.maps.DistanceMatrixStatus.OK){for(var a,o,i=Number.POSITIVE_INFINITY,s=e.rows[0].elements,r=0;r<s.length;r++)o=s[r].duration.value,i>o&&(i=o,a=r);n.removeClass("is-loading"),setActiveCampus(templateData.campuses[a].id)}}function a(){for(var e=[],t=0;t<templateData.campuses.length;t++){var a=templateData.campuses[t].geo,n=new google.maps.LatLng(a.lat,a.lng);e.push(n)}return e}var n=$(e),o=new google.maps.DistanceMatrixService;navigator.geolocation&&(n.addClass("is-loading"),navigator.geolocation.getCurrentPosition(function(e){var n=new google.maps.LatLng(e.coords.latitude,e.coords.longitude);o.getDistanceMatrix({origins:[n],destinations:a(),travelMode:google.maps.TravelMode.DRIVING},t)}))}var md=new MobileDetect(window.navigator.userAgent);Handlebars.registerHelper("isPhone",function(e){return md.phone()?e.fn(this):e.inverse(this)}),Handlebars.registerHelper("rowSpan",function(e){return e.length+1}),Handlebars.registerHelper("mapUrl",function(){var e=encodeURIComponent(this.street+" "+this.city+", "+this.state+" "+this.zip);return"http://maps.apple.com/maps?daddr="+e}),Handlebars.registerHelper("removeAst",function(e){return e.replace(" *","")}),Handlebars.registerHelper("emailSingle",function(e,t,a){return"mailto:?subject="+encodeURIComponent(text.emailSubject)+"&body="+encodeURIComponent(text.emailSingleP1)+encodeURIComponent(e)+encodeURIComponent(text.emailSingleP2)+encodeURIComponent(t)+encodeURIComponent(text.emailSingleP3)+encodeURIComponent(a)+encodeURIComponent(text.emailSingleP4)}),Handlebars.registerHelper("emailAll",function(){var e="";if(e+=text.emailAllP1,e+=this.name,e+=text.emailAllP2,this.times.tue){e+="\n"+this.tueLabel+"\n";for(var t=0;t<this.times.tue.length;t++){var a=this.times.tue[t];e+="- "+a+"\n"}}if(this.times.wed){e+="\n"+this.wedLabel+"\n";for(var t=0;t<this.times.wed.length;t++){var a=this.times.wed[t];e+="- "+a+"\n"}}if(this.times.thu){e+="\n"+this.thuLabel+"\n";for(var t=0;t<this.times.thu.length;t++){var a=this.times.thu[t];e+="- "+a+"\n"}}e+=this.hasBothSpecialService?text.emailAllP3b:text.emailAllP3;var n="mailto:?subject="+encodeURIComponent(text.emailSubject)+"&body="+encodeURIComponent(e);return n}),Handlebars.registerHelper("textSingle",function(e,t,a){var n=text.textSingleP1+e+text.textSingleP2+t+text.textSingleP3+a+text.textSingleP4;return"sms:&body="+encodeURIComponent(n)}),Handlebars.registerHelper("textAll",function(){var e=text.textAllP1+this.name+text.textAllP2;return"sms:&body="+encodeURIComponent(e)});var source=$("#campuses-template").html(),template=Handlebars.compile(source);$("#campuses-container").append(template(templateData)),$(document).ready(function(){$("body").on("click",".campus .title",function(){var e=$(this);e.siblings(".collapse").collapse("toggle"),e.parents(".campus").toggleClass("open")}),$("#inviteModal").on("show.bs.modal",function(e){var t=$(e.relatedTarget),a=t.data("invite-method"),n="",o=t.data("campus-id"),i=templateData.campuses.filter(function(e){return e.id===o})[0];switch(i.tueLabel=templateData.tueLabel,i.wedLabel=templateData.wedLabel,i.thuLabel=templateData.thuLabel,a){case"email":n="Send an email invite";break;case"text":n="Send a text message invite";break;case"facebook":n="Share on Facebook"}var s=$("#"+a+"-invite-template").html(),r=Handlebars.compile(s),l=$(this);l.find(".modal-title").text(n),l.find(".modal-body").html(r(i))})});var setActiveCampus=function(e){var t=$(".campus.active");t.length&&t.removeClass("active open").find(".collapse").removeClass("in"),$(".campus[data-campus-id="+e+"]").addClass("active open").insertBefore(".campus:first").find(".collapse").addClass("in"),Cookies.set("christmas_campus",e)},cookie=Cookies.get("christmas_campus");cookie&&setActiveCampus(cookie),navigator.geolocation&&$(".js-find-nearest-container").html('<button type="button" class="btn btn-default find-nearest" onClick="findNearestCampus(this)"><i class="loading-icon"></i> Find Nearest Campus</button>'),TweenMax.fromTo(".a-swing",2,{rotation:-30},{rotation:30,transformOrigin:"top center",ease:Quad.easeInOut,repeat:-1,yoyo:!0}),TweenMax.fromTo(".a-swing-delay",2,{rotation:-30},{rotation:30,transformOrigin:"top center",ease:Quad.easeInOut,delay:2,repeat:-1,yoyo:!0}),TweenMax.to(".a-spin-left",12,{rotation:-360,transformOrigin:"center center",ease:Linear.easeNone,repeat:-1}),TweenMax.to(".a-spin-right",12,{rotation:360,transformOrigin:"center center",ease:Linear.easeNone,repeat:-1}),TweenMax.fromTo(".a-pulse-1",2,{scale:.75},{scale:1.25,transformOrigin:"center center",repeat:-1,yoyo:!0}),TweenMax.fromTo(".a-pulse-2",2,{scale:.75},{scale:1.25,transformOrigin:"center center",repeat:-1,yoyo:!0,delay:1}),TweenMax.fromTo(".a-pulse-3",2,{scale:.75},{scale:1.25,transformOrigin:"center center",repeat:-1,yoyo:!0,delay:2}),TweenMax.to(".a-burst-l",3,{scale:1.3,transformOrigin:"center center",repeat:-1,ease:Quad.easeInOut,yoyo:!0});