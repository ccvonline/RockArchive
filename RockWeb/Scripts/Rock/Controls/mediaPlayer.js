﻿(function ($) {
    'use strict';
    window.Rock = window.Rock || {};
    Rock.controls = Rock.controls || {};

    Rock.controls.mediaPlayer = (function () {
        var exports = {
            initialize: function () {

                Sys.Application.add_load(function () {
                    var cssFile = Rock.settings.get('baseUrl') + 'Scripts/mediaelementjs/mediaelementplayer.min.css';
                    var jsFile = Rock.settings.get('baseUrl') + 'Scripts/mediaelementjs/mediaelement-and-player.js';

                    // ensure that css for mediaelementplayers is added to page
                    if (!$('#mediaElementCss').length) {
                        $('head').append("<link id='mediaElementCss' href='" + cssFile + "' type='text/css' rel='stylesheet' />");
                    }

                    // ensure that js for mediaelementplayers is added to page
                    if (!$('#mediaElementJs').length) {
                        $('head').prepend("<script id='mediaElementJs' src='" + jsFile + "' />");
                    }

                    // ensure that mediaelementplayer is applied to all audio/video on the page
                    $('audio,video').mediaelementplayer({ enableAutosize: true });
                });

            }
        };

        return exports;
    }());
}(jQuery));