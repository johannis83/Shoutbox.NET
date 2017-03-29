var initializeTagCloud = function () {
    $("#tagcloud a").tagcloud({
        size: {
            start: 10,
            end: 30,
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

/* This will update the tagcloud dynamically. If the tag already exists, it'll update the weight
   Otherwise it will be appended to the list of tags */
var updateTagCloud = function (newTag) {
    var tagfound = false;
    $("#tagcloud").children('a').each(function () {
        var tag = $(this).text();
        var weight = $(this).attr('rel');

        //Existing tag? Do not append it but increase the weight of the existing one
        if ("#" + newTag == tag) {
            $(this).attr('rel', (parseInt($(this).attr('rel')) + 1));
            initializeTagCloud();
            tagfound = true; 
        }
    });

    if (tagfound) { return; }

    //New tag? add it
    $("#tagcloud").append('<a href="/Tag/' + newTag + '" rel="' + 1 + '">#'+newTag+'</a>');
    initializeTagCloud();
}