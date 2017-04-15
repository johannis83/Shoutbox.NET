﻿var messages;

$.fn.ProcessInput = function (event, type) {
    var textbox = $(this);
    var tagDisplay = textbox.parent().find(".tag-display");

    var textboxvalue = $(textbox).val();
    var textboxwords = textboxvalue.split(' ');

     // 13 = Enter
    if (event.keyCode == '13') {
        //Valid tag is required on announcement mssages
        if (tagDisplay.html().length < 3 && type == 'Announcement') { return; }
        //Message requires atleast 4 characters
        if (textboxvalue.length < 3) { return; }

        sendMessage($(tagDisplay).text(), $(textbox).val(), type);
        tagDisplay.html("");
        tagDisplay.removeClass("tag-word-active");
        $(textbox).val("");
    }

    // Remove the defined tag if backspace is pressed while the textbox is empty
    else if (textboxvalue == "") {
        // 8 = Backspace
        if (event.keyCode == '8') {
            tagDisplay.html("");
            tagDisplay.removeClass("tag-word-active");
        }
    }
    //Use the first typed word as hashtag
    else if (type == 'Announcement') {
        if (textboxwords.length > 1 && tagDisplay.hasClass('tag-word-active') == false) {
            tagDisplay.html("#" + textboxwords[0]
                .toUpperCase()                   // Force uppercasing of hashtags
                .replace(/[^0-9a-zA-Z]/g, '')); // Only alphanumeric characters
            tagDisplay.addClass("tag-word-active")
            $(textbox).val(textboxwords.slice(1, textboxwords.length).join(" "));
        }
    }
}

//Rescroll the chat down when resizing the window
$(window).resize(function () {
    clearTimeout(window.resizedFinished);
    window.resizedFinished = setTimeout(function () {
        scrollWindowsToBottom(0);
    }, 100);
});

    $(function () {
        var options = {
            cellHeight: 80,
            animate: true,
            verticalMargin: 10,
            draggable: {
                handle: '.tile-titlebar',
            },
        };
        $('.grid-stack').gridstack(options);
    });


    //Must be hooked to a #chat-window object
    $.fn.AddMessage = function (name, division, time, tag, text, type, autoscroll) {

        //If it's not destined for our announcementchannel, don't add it.
        if (type == "Announcement" && division != AnnouncementChannel)
            return;

        var chatTile = $(this);
        var messageContainer = chatTile.find(".message-container");
        //Keep count of all the message in this container
        var messageCount = chatTile.find(".message-counter").get(0).value++;

        //Hide (fade) the chatbox filler if no were present yet
        if (messageCount == 0) {
            chatTile.find(".chat-filler").fadeTo(1000, 0);
        } //else if (messageCount > 7) {
        //    //Remove the chat filler if the messagebox is filled up with messages
        //    chatTile.find(".chat-filler").hide();
        //}

        messageContainer.append(messageTemplate(name, division, time, tag, text, type));
        $("abbr.timeago").timeago();

        if (autoscroll) {
            $(chatTile).parent().stop().animate({ scrollTop: $(chatTile).prop("scrollHeight") }, 5000, 'easeOutQuart');
        }
    }


    var messageTemplate = function (name, division, time, tag, text, type) {

        var uppercaseFirstCharacterTag = tag.toLowerCase().charAt(0).toUpperCase() + tag.slice(1);
        var message = "";
        message = message.concat('<!-- Begin message -->');

        if(type == "Chat")
            message = message.concat('<div id="chat-message" class="well">');
        else
            message = message.concat('<div id="announcement-message" class="well">');

            message = message.concat('<div id="message-header">');
            message = message.concat('<div id="message-name">' + name + '</div>');

            //Dutch users get orange badges. Hup oranje!
            if (division == "RN") {
                message = message.concat('<div id="message-division" class="badge" style="background-color: #fd4600">' + division + '</div>');
            } else {
                message = message.concat('<div id="message-division" class="badge">' + division + '</div>');
            }

            message = message.concat('<div id="message-time"><i class="fa fa-clock-o" aria-hidden="true"></i><abbr class="timeago" title="' + time + '">' + time + ' </abbr> </div>');
            message = message.concat('</div>');
            message = message.concat('<div id="message-content">');
            //Only add a tag label if a tag is specified. Chat messages don't use tags, so..
            if(tag != "") message = message.concat('<div id="message-tag" onclick="location.href=\'Tag/' + uppercaseFirstCharacterTag + '\';" class="label label-primary">' + tag + '</div>');
            message = message.concat('<p id="message-text">' + text);
            message = message.concat('</p>');
            message = message.concat('</div>');
            message = message.concat('</div>');
            message = message.concat('<!-- End message -->');
            return message;
        }


    var addAnnouncementMessages = function (messages, autoscroll) {
        for (var i = 0; i < messages.length; i++) {
            if (messages[i]["Type"] == "Announcement") {
                $("#announcement-window").AddMessage(messages[i]["User"]["Name"], messages[i]["User"]["Division"], messages[i]["Timestamp"], messages[i]["Tag"], messages[i]["Text"], messages[i]["Type"], false);
            }
        }
        //Scroll both chat containers down
        if (autoscroll) {
            scrollWindowsToBottom(1000);
        }
    }

    addMessages = function (messages, autoscroll) {
        for (var i = 0; i < messages.length; i++) {
            //Add to the appropriate message window depending on the messages type
            if (messages[i]["Type"] == "Chat") {
                $("#chat-window").AddMessage(messages[i]["User"]["Name"], messages[i]["User"]["Division"], messages[i]["Timestamp"], messages[i]["Tag"], messages[i]["Text"], messages[i]["Type"], false);
            } else if (messages[i]["Type"] == "Announcement") {
                $("#announcement-window").AddMessage(messages[i]["User"]["Name"], messages[i]["User"]["Division"], messages[i]["Timestamp"], messages[i]["Tag"], messages[i]["Text"], messages[i]["Type"], false);
            }
        }
        //Scroll both chat containers down
        if (autoscroll) {
            scrollWindowsToBottom(1000);
        }
    };

    addMessagesTagViewer = function (messages, autoscroll) {
        for (var i = 0; i < messages.length; i++) {
            //Add to the appropriate message window depending on the messages type
            $("#chat-window").AddMessage(messages[i]["User"]["Name"], messages[i]["User"]["Division"], messages[i]["Timestamp"], messages[i]["Tag"], messages[i]["Text"], messages[i]["Type"], false);
        }

        if (autoscroll) {
            $("#chat-window").parent().stop().animate({ scrollTop: $("#chat-window").prop("scrollHeight") }, 3000, 'easeOutQuart');
        }
    }


var clearAnnouncements = function () {
    $("div[id=announcement-message]").remove();
    $("#announcement-window").find(".message-counter").get(0).value = 0;
}

var scrollWindowsToBottom = function (duration) {
    $("#announcement-window").parent().stop().animate({ scrollTop: $("#announcement-window").prop("scrollHeight") }, duration, 'easeOutQuart');
    $("#chat-window").parent().stop().animate({ scrollTop: $("#chat-window").prop("scrollHeight") }, duration, 'easeOutQuart');
}

var setChannelToggle = function (division) {
    if (division == "RN")
        $('#channel-toggle').bootstrapToggle('on')
    else if (division == "WRR")
        $('#channel-toggle').bootstrapToggle('off')
}


$(function() {
    $('#channel-toggle').change(function () {

        var messages;

        clearAnnouncements();
        if ($(this).prop('checked') == true) {
            AnnouncementChannel = "RN"
        }
        else {
            AnnouncementChannel = "WRR";
        }


        $.post('/Message/GetTodayByDivisionSerialized', { division: AnnouncementChannel }, function (data) {
            addAnnouncementMessages(JSON.parse(data), true);
        });

    })
})




// Instantiate nice scroll
$(document).ready(function () {
    $(".grid-stack-item-content").niceScroll({
        scrollspeed: 10,
        mousescrollstep: 100,
        enablescrollonselection: false,
        cursorwidth: 5,
        cursorborder: 0,
        cursorcolor: '#d8d8d8',
        cursorborderradius: 0,
        autohidemode: true,
        horizrailenabled: false,
        touchbehavior: false,
        grabcursorenabled: false,
        cursordragontouch: false
    });

});
