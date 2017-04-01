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
    //On setting the hashtag
    else if (textboxvalue.charAt(0) == '#') {
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

        //Tag is required
        if (tagDisplay.html() == "") { return; }
        //Message requires atleast 5 characters
        if (textboxvalue.length < 5) { return; } 

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

        var uppercaseFirstCharacterTag = tag.toLowerCase().charAt(0).toUpperCase() + tag.slice(1);

        var messageTemplate =
            '<!-- Begin message -->' +
            '<div id="message">' +
            '<div id="message-name">' + name + '</div>' +
            '<div id="message-division" class="badge">' + division + '</div>' +
            '<div id="message-time"><i class="fa fa-clock-o" aria-hidden="true"></i><abbr class="timeago" title="' + time + '">' + time + ' </abbr> </div>' +
            '<div id="message-content">' +
            '<div id="message-tag" onclick="location.href=\'/Tag/' + uppercaseFirstCharacterTag +'\';" class="label label-primary">' + tag + '</div>' +
            '<div id="message-text">' +
            text +
            '</div>' +
            '</div>' +
            '<hr>' +
            '</div>' +
            '<!-- End message -->';

        messageContainer.append(messageTemplate);

        if (autoscroll) {
            $(chatTile).parent().stop().animate({ scrollTop: $(chatTile).prop("scrollHeight") }, 5000, 'easeOutQuart');
        }
        jQuery("abbr.timeago").timeago();
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
            if (autoscroll) {
                $(this).parent().animate({ scrollTop: $(this).prop("scrollHeight") }, 500, 'easeOutQuart');
            }
    };

    addMessagesTagViewer = function (messages, autoscroll) {
        for (var i = 0; i < messages.length; i++) {
            //Add to the appropriate message window depending on the messages type
            $("#chat-window").AddMessage(messages[i]["User"]["Name"], messages[i]["User"]["Division"], messages[i]["Timestamp"], messages[i]["Tag"], messages[i]["Text"], false);
        }

        if (autoscroll) {
            //  $(this).parent().animate({ scrollTop: $(this).prop("scrollHeight") }, 500, 'easeOutQuart');
        }
    }
//Extension method to make a tile autoscroll to the bottom
$.fn.ScrollToBottom = function () {
    $(this).parent().getNiceScroll(0).doScrollTop($(this).height(), 5000);
};

// Instantiate nice scroll
$(document).ready(function () {
    $("#chat-window").parent().niceScroll({
        cursorwidth: 5,
        cursorborder: 0,
        cursorcolor: '#d8d8d8',
        cursorborderradius: 0,
        autohidemode: true,
        horizrailenabled: false
    });

    $("#announcement-window").parent().niceScroll({
        cursorwidth: 5,
        cursorborder: 0,
        cursorcolor: '#d8d8d8',
        cursorborderradius: 0,
        autohidemode: true,
        horizrailenabled: false
    });

    //Auto scroll to bottom
    $('#chat-window').ScrollToBottom();
    $('#announcement-window').ScrollToBottom();
});
