var updateKM = function (KMList) {

    //Clear current KM before appending new ones
    $("#KM-table-body").empty();

    //Add them
    for (var i = 0; i < KMList.length / 2; i++) {
        $("#KM-table-body").append(
            KMRowTemplate(KMList[i]["Nummer"], KMList[i]["Titel"], KMList[i]["AantalMeldingen"],
                KMList[i + KMList.length / 2 - 1]["Nummer"], KMList[i + KMList.length / 2 - 1]["Titel"], KMList[i + KMList.length / 2 - 1]["AantalMeldingen"]));

        //Setup tool tips so descriptions of KM' can be read when hovering over them
        $("#KM-table-body tr td").tooltipster({
            theme: 'tooltipster-light',
            maxWidth: 500,
            debug: false,
        });
    }
}

var KMRowTemplate = function (nummer, titel, aantal, nummer2, titel2, aantal2) {
    var template = "";

    template = template.concat('<tr>');
    template = template.concat('<td title="' + nummer + ': ' + titel + '"> <span class="label label-default">' + aantal + '</span> ' + nummer + '</td>');
    template = template.concat('<td title="' + nummer2 + ': '+ + titel2 + '"> <span class="label label-default">' + aantal2 + '</span> ' + nummer2 + '</td>');
    template = template.concat('<tr>');

    return template;
}