$.fn.ProcessInput = function (event, type) {
    var textbox = $(this);
    var tagDisplay = textbox.parent().find(".tag-display");

    var textboxvalue = $(textbox).val();
    var textboxwords = textboxvalue.split(' ');
    // Remove the defined tag if backspace is pressed while the textbox is empty
    if (textboxvalue == "") {
        // 8 = Backspace
        if (event.keyCode == '8') {
            tagDisplay.html("");
            tagDisplay.removeClass("tag-word-active");
        }
    }
    //On setting a hashtag, only announcement messages can be tagged
    else if (textboxvalue.charAt(0) == '#' && type == 'Announcement') {
        if (textboxwords.length > 1) {
            tagDisplay.html(htmlEncode(textboxwords[0].substring(1))
                .toUpperCase()                   // Force uppercasing of hashtags
                .replace(/[^0-9a-zA-Z]/g, '')); // Only alphanumeric characters
            tagDisplay.addClass("tag-word-active")
            $(textbox).val(textboxwords.slice(1, textboxwords.length).join(" "));
        }
    }

    // 13 = Enter
    else if (event.keyCode == '13') {
        //Tag is required on announcement mssages
        if (tagDisplay.html() == "" && type == 'Announcement') { return; }
        //Message requires atleast 4 characters
        if (textboxvalue.length < 4) { return; } 
        
        sendMessage($(tagDisplay).text(), $(textbox).val(), type);
        tagDisplay.html("");
        tagDisplay.removeClass("tag-word-active");
        $(textbox).val("");
    }

}

    $(function () {
        var options = {
            cellHeight: 80,
            verticalMargin: 10,
            draggable: {
                handle: '.tile-titlebar',
            },
        };
        $('.grid-stack').gridstack(options);
    });


    var UpdateScrollBar = function (obj) {
        obj.getNiceScroll().resize();
    }

    //Must be hooked to a #chat-window object
    $.fn.AddMessage = function (name, division, time, tag, text, autoscroll) {
        var chatTile = $(this);
        var messageContainer = chatTile.find(".message-container");
        //Keep count of all the message in this container
        var messageCount = chatTile.find(".message-counter").get(0).value++;

        //Hide (fade) the chatbox filler if no were present yet
        if (messageCount == 0) {
            chatTile.find(".chat-filler").fadeTo(1000, 0);
        }

        messageContainer.append(messageTemplate(name, division, time, tag, text));

        if (autoscroll) {
            $(chatTile).parent().stop().animate({ scrollTop: $(chatTile).prop("scrollHeight") }, 5000, 'easeOutQuart');
        }
        jQuery("abbr.timeago").timeago();
    }


    var messageTemplate = function (name, division, time, tag, text) {
        var uppercaseFirstCharacterTag = tag.toLowerCase().charAt(0).toUpperCase() + tag.slice(1);
        var message = "";
            message = message.concat('<!-- Begin message -->');
            message = message.concat('<div id="message">');
            message = message.concat('<div id="message-name">' + name + '</div>');
            message = message.concat('<div id="message-division" class="badge">' + division + '</div>');
            message = message.concat('<div id="message-time"><i class="fa fa-clock-o" aria-hidden="true"></i><abbr class="timeago" title="' + time + '">' + time + ' </abbr> </div>');
            message = message.concat('<div id="message-content">');
            //Only add a tag label if a tag is specified. Chat messages don't use tags, so..
            if(tag != "") message = message.concat('<div id="message-tag" onclick="location.href=\'Tag/' + uppercaseFirstCharacterTag + '\';" class="label label-primary">' + tag + '</div>');
            message = message.concat('<div id="message-text">' + text);
            message = message.concat('</div>');
            message = message.concat('</div>');
            message = message.concat('<hr>');
            message = message.concat('</div>');
            message = message.concat('<!-- End message -->');

            return message;
        }

    addMessages = function (messages, autoscroll) {
        for (var i = 0; i < messages.length; i++) {
            //Add to the appropriate message window depending on the messages type
            if (messages[i]["Type"] == "Chat") {
                $("#chat-window").AddMessage(messages[i]["User"]["Name"], messages[i]["User"]["Division"], messages[i]["Timestamp"], messages[i]["Tag"], messages[i]["Text"], false);
            } else if (messages[i]["Type"] == "Announcement") {
                $("#announcement-window").AddMessage(messages[i]["User"]["Name"], messages[i]["User"]["Division"], messages[i]["Timestamp"], messages[i]["Tag"], messages[i]["Text"], false);
            }
        }
                //Scroll both chat containers down
                if (autoscroll) {
                    $("#announcement-window").parent().stop().animate({ scrollTop: $("#announcement-window").prop("scrollHeight") }, 1000, 'easeOutQuart');
                    $("#chat-window").parent().stop().animate({ scrollTop: $("#chat-window").prop("scrollHeight") }, 1000, 'easeOutQuart');
                }
    };

    addMessagesTagViewer = function (messages, autoscroll) {
        for (var i = 0; i < messages.length; i++) {
            //Add to the appropriate message window depending on the messages type
            $("#chat-window").AddMessage(messages[i]["User"]["Name"], messages[i]["User"]["Division"], messages[i]["Timestamp"], messages[i]["Tag"], messages[i]["Text"], false);
        }

        if (autoscroll) {
            $("#chat-window").parent().stop().animate({ scrollTop: $("#chat-window").prop("scrollHeight") }, 3000, 'easeOutQuart');
        }
    }
//Extension method to make a tile autoscroll to the bottom
$.fn.ScrollToBottom = function () {
    $(this).parent().getNiceScroll(0).doScrollTop($(this).height(), 5000);
};

// Instantiate nice scroll
$(document).ready(function () {
    $(".grid-stack-item-content").niceScroll({
        cursorwidth: 5,
        cursorborder: 0,
        cursorcolor: '#d8d8d8',
        cursorborderradius: 0,
        autohidemode: true,
        horizrailenabled: false,
        touchbehavior: false,
        grabcursorenabled: false
    });

    //Auto scroll to bottom
    $('#chat-window').ScrollToBottom();
    $('#announcement-window').ScrollToBottom();
});
