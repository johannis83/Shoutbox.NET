$.fn.ProcessInput = function (event, textbox) {
    textbox = $(this);
    var tagDisplay = textbox.parent().find(".tag-display");

    var textboxvalue = $(textbox).val();
    var textboxwords = textboxvalue.split(' ');

    // Remove the defined tag if backspace is pressed while the textbox is empty
    if (textboxvalue == "") {
        // 8 = Backspace
        if (event.keyCode === '8') {
            tagDisplay.html("");
            tagDisplay.removeClass("tag-word-active");
        }
    }

    else if (textboxvalue.charAt(0) == '#') {
        if (textboxwords.length > 1) {
            tagDisplay.html(htmlEncode(textboxwords[0].substring(1)));
            tagDisplay.addClass("tag-word-active")
            $(textbox).val(textboxwords.slice(1, textboxwords.length).join(" "));
        }
    }
    // 13 = Enter
    else if (event.keyCode == '13') {
        //Get the chat tile that belongs to this textbox
        //textbox.parentsUntil(".chat-tile").AddMessage("Bubuchenko", "WRR", new Date(), $(tagDisplay).text(), $(textbox).val(), true);


        sendChatMessage($(tagDisplay).text(), $(textbox).val());

        tagDisplay.html("");
        tagDisplay.removeClass("tag-word-active");
        $(textbox).val("");
    }

}

    $(function () {
        var options = {
        cellHeight: 80,
            verticalMargin: 10,
            horizontalMargin: 0
        };
        $('.grid-stack').gridstack(options);
    });


    var UpdateScrollBar = function (obj) {
        obj.getNiceScroll().resize();
    }

    $.fn.AddMessage = function (name, division, time, tag, text, autoscroll) {
        var chatTile = $(this);
        var messageContainer = chatTile.find(".messages-container");

        time = jQuery.timeago(time);
        var messageTemplate =
            '<!-- Begin message -->' +
            '<div id="message">' +
            '<div id="message-name">' + name + '</div>' +
            '<div id="message-division" class="badge">' + division + '</div>' +
            '<div id="message-time"><i class="fa fa-clock-o" aria-hidden="true"></i><abbr datetime="' + time + '">' + time + ' </abbr> </div>' +
            '<div id="message-content">' +
            '<div id="message-tag" class="label label-primary">' + tag + '</div>' +
            '<div id="message-text">' +
            text +
            '</div>' +
            '</div>' +
            '<hr>' +
            '</div>' +
            '<!-- End message -->';

        messageContainer.append(messageTemplate);

        if (autoscroll) {
            setTimeout(function () {
                chatTile.animate({
                    scrollTop: chatTile[0].scrollHeight
                }, 500);
            }, 10);
        }
    }

    // Instantiate nice scroll
    $(document).ready(function () {
            $("#chat-tile").niceScroll({
                cursorwidth: 5,
                cursorborder: 0,
                cursorcolor: '#d8d8d8',
                cursorborderradius: 0,
                autohidemode: true,
                horizrailenabled: false
            });

        //Autoscroll chatbox down upon loading the page
        $('#chat-tile').getNiceScroll(0).doScrollTop($('#chat-tile').height());
    });