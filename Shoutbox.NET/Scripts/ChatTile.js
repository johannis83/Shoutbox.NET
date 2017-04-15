var messages;

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
                .toUpperCase()
                .substring(0, 12)             // Force uppercasing of hashtags
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
    $.fn.AddMessage = function (id, name, division, time, tag, text, type, relevant, autoscroll) {

        //If it's not destined for our announcementchannel, don't add it.

        var chatTile = $(this);
        var messageContainer = chatTile.find(".message-container");
        //Keep count of all the message in this container
        var messageCount = chatTile.find(".message-counter").get(0).value++;

        //Hide (fade) the chatbox filler if no were present yet
        if (messageCount == 0) {
            chatTile.find(".chat-filler").fadeTo(1000, 0);
        }

        var newMessage = messageTemplate(id, name, division, time, tag, text, type, relevant);

        //If we're on the announcement channel for this message, show it. Otherwise don't
        if (division == AnnouncementChannel && type == "Announcement") {
            newMessage = $(newMessage);        //None used for history view
        } else if (type == "Announcement" && AnnouncementChannel != "None") {
            newMessage = $(newMessage).css("display", "none");
        }

        messageContainer.append(newMessage);


        if (!relevant) {
            $(".message-" + id).find("#message-tag").css("text-decoration", "line-through");
            $(".message-" + id).css("text-decoration", "line-through");
        }

        $("abbr.timeago").timeago();

        if (autoscroll) {
            $(chatTile).parent().stop().animate({ scrollTop: $(chatTile).prop("scrollHeight") }, 5000, 'easeOutQuart');
        }
    }


    var messageTemplate = function (id, name, division, time, tag, text, type, relevant) {

        var uppercaseFirstCharacterTag = tag.toLowerCase().charAt(0).toUpperCase() + tag.slice(1);
        var message = "";
        message = message.concat('<!-- Begin message -->');

        if(type == "Chat")
            message = message.concat('<div class="chat-message well">');
        else
            if(division == "RN")
                message = message.concat('<div class="announcement-message RN-Message well">');
            else if (division == "WRR")
                message = message.concat('<div class="announcement-message WRR-Message well">');

            message = message.concat('<div id="message-header" class="message-' + id + '">');
            message = message.concat('<div id="message-name">' + name + '</div>');

            //Dutch users get orange badges.
            if (division == "RN") {
                message = message.concat('<div id="message-division" class="badge" style="background-color: #fd4600">' + division + '</div>');
            } else {
                message = message.concat('<div id="message-division" class="badge">' + division + '</div>');
            }

            message = message.concat('<div id="message-time"><i class="fa fa-clock-o" aria-hidden="true"></i><abbr class="timeago" title="' + time + '">' + time + ' </abbr> </div>');
            message = message.concat('</div>');
            message = message.concat('<div id="message-content" class="message-' + id + '">');
            //Only add a tag label if a tag is specified. Chat messages don't use tags, so..
            if (tag != "") message = message.concat('<div id="message-tag" onclick="disableMessageRelevance(' + id + ')" class="label label-primary">' + tag + '</div>');
            message = message.concat('<p id="message-text">' + text);
            message = message.concat('</p>');
            message = message.concat('</div>');
            message = message.concat('</div>');
            message = message.concat('<!-- End message -->');
            return message;
        }


    addMessages = function (messages, autoscroll) {
        for (var i = 0; i < messages.length; i++) {
            //Add to the appropriate message window depending on the messages type
            if (messages[i]["Type"] == "Chat") {
                $("#chat-window").AddMessage(messages[i]["MessageID"], messages[i]["User"]["Name"], messages[i]["User"]["Division"], messages[i]["Timestamp"], messages[i]["Tag"], messages[i]["Text"], messages[i]["Type"], messages[i]["Relevant"], false);
            } else if (messages[i]["Type"] == "Announcement") {
                $("#announcement-window").AddMessage(messages[i]["MessageID"], messages[i]["User"]["Name"], messages[i]["User"]["Division"], messages[i]["Timestamp"], messages[i]["Tag"], messages[i]["Text"], messages[i]["Type"], messages[i]["Relevant"], false);
            }
        }
        //Scroll both chat containers down
        if (autoscroll) {
            scrollWindowsToBottom(1000);
        }
    };

var disableMessageRelevance = function (id) {
    if (userRole != "User") {
        sendDisableMessage(id);
    }
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

$(function () {
    $('#channel-toggle').change(function () {
        var messages;

        if ($(this).prop('checked') == true) {
            AnnouncementChannel = "RN";
            $(".WRR-Message").fadeOut("fast", function () {
                $(".RN-Message").fadeIn("fast");
            });

            //Call it again because if no messages are present yet, the first fadein won't be called
            $(".RN-Message").fadeIn("fast");
        }
        else {
            AnnouncementChannel = "WRR";
            $(".RN-Message").fadeOut("fast", function () {
                $(".WRR-Message").fadeIn("fast");
            });

            $(".WRR-Message").fadeIn("fast");
        }

        $("#announcement-window").parent().stop().animate({ scrollTop: $("#announcement-window").prop("scrollHeight") }, 1000, 'easeOutQuart');
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


