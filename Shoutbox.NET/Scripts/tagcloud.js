var initializeTagCloud = function () {
    $("#tagcloud a").tagcloud({
        size: {
            start: 8,
            end: 15,
            unit: 'pt'
        },
        color: {
            start: '#bfbfbf',
            end: '#009'
        },
        weight: {
            start: 100,
            end: 900
        }
    });
}

// Instantiate nice scroll
$(document).ready(function () {
    $("#tagcloud").niceScroll({
        cursorwidth: 5,
        cursorborder: 0,
        cursorcolor: '#d8d8d8',
        cursorborderradius: 0,
        autohidemode: true,
        horizrailenabled: false
    });
});


var fillTagCloud = function (tags) {
    //Clear old tags, if any
    $("#tagcloud a").remove();

    //Show tile filler, if nothing is tagged
    if (tags.length == 0) {
        $("#tagcloud").append('<div class="tagcloud-filler"> <p>Nog geen relevante topics</p></div>');
        return;
    }
    for (var i = 0; i < tags.length; i++) {
        $("#tagcloud").append(
            '<a rel="' + tags[i]["Count"] + '" title="' + tags[i]["Count"] + ' tags" id="tag-' + tags[i]["Name"] + '">#' + tags[i]["Name"] + '</a>');
    }
}

var unTag = function (tag) {
    var tagElement = $("#tag-" + tag);
    var weight = parseInt(tagElement.attr('rel'));

    if (weight == 1) {
        tagElement.remove();
        if ($("#tagcloud").find('a').length == 0) {
            $("#tagcloud").append('<div class="tagcloud-filler"> <p>Geen relevante topics</p></div>');
        }
        return;
    }

    tagElement.attr('rel', weight - 1);
    tagElement.attr('title', weight - 1 + " tags");
    initializeTagCloud();
}

/* This will update the tagcloud dynamically. If the tag already exists, it'll update the weight
   Otherwise it will be appended to the list of tags */
var updateTagCloud = function (newTag) {
    var tagfound = false;

    if ((newTag == '#') || (newTag == '')) return;

    $(".tagcloud-filler").remove();

    $("#tagcloud").children('a').each(function () {
        var tag = $(this).text();
        var weight = parseInt($(this).attr('rel'));

        //Existing tag? Do not append it but increase the weight of the existing one
        if ("#" + newTag == tag) {
            $(this).attr('rel', weight + 1);
            $(this).attr('title', weight + 1 + " tags");
            initializeTagCloud();
            tagfound = true; 
        }
    });

    if (tagfound) { return; }

    //New tag? add it
    $("#tagcloud").append(
        '<a rel="1" title=1" id="tag-' + newTag + '">#' + newTag + '</a>');
    initializeTagCloud();
}