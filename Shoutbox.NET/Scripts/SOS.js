var updateSOS = function (SOSList) {

    if (SOSList.length > 0) {

        //Hide our filler if there are messages
        $("#sos-filler").addClass("invisible");
        //Show our table
        $("#sos-table").removeClass("invisible");

        //Clear current SOS before appending new ones
        $("#SOS-table-body").empty();

        //Add them
        for (var i = 0; i < SOSList.length; i++) {
            $("#SOS-table-body").append(SOSTemplate(SOSList[i]["Name"], SOSList[i]["Description"], SOSList[i]["Time"]));
            jQuery("#SOS-table-body time.timeago").timeago();

            //Setup tool tips so descriptions of SOS' can be read when hovering over them
            $("#SOS-table-body > tr").tooltipster({
                theme: 'tooltipster-light',
                maxWidth: 500,
                debug: false
            });
        }

    }
    else {
        //Show our filler if there are no messages
        $("#sos-table").addClass("invisible");
        //Show our filler, if it's hidden
        $("#sos-filler").removeClass("invisible");
    }
}

var SOSTemplate = function(name, description, time) {
    var template = "";

    template = template.concat('<tr title="' + description +  '">');
    template = template.concat('<td>' + name + '</td>');
    template = template.concat('<td class="SOS-Time"><i class="fa fa-clock-o" aria-hidden="true"><time class="timeago" datetime="'+time+'">'+time+'</time></td>"');
    template = template.concat('<tr>');

    return template;
}