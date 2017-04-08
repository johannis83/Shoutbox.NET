var createMasterIncident = function (description, km, im) {
    swal.setDefaults({
        input: 'text',
        confirmButtonText: 'Next &rarr;',
        showCancelButton: true,
        animation: false,
        progressSteps: ['OM', 'KM', 'IM'],

        preConfirm: function (input) {
            return new Promise(function (resolve, reject) {
                if (input.length <= 3) {
                    reject("Geef een geldige waarde op!");
                } else {
                    resolve();
                }
            })
        }
    })

    var steps = [
        {
            title: 'Nieuw master incident plaatsen',
            text: 'Geef een omschrijving',
            animation: true,
        },
        'Geef het KM nummer op',
        'Geef het IM nummer op',
    ]

    swal.queue(steps).then(function (result) {
        swal.resetDefaults()
        swal({
            title: 'Wil je het volgende master incident plaatsen?',
            showCancelButton: true,
            type: 'warning',
            html:
            '<br>' + result[0] +
            '<br>Gebruik: ' + result[1] +
            '<br>Relateer: ' + result[2],
            confirmButtonText: 'Prima!',
            confirmButtonColor: '#5cb85c',
            cancelButtonText: 'Nee',
            cancelButtonColor: '#d33',
        }).then(function () {

            sendCreateMasterIncident(result[0], result[1], result[2]);

            swal({
                title: "Done!",
                text: "Het nieuwe master incident is geplaatst",
                type: "success",
                confirmButtonText: 'Top!',
                showCancelButton: false,
            });
        });
    });
}

var addMasterIncident = function (id, description, km, im, time) {
    $('.incident-container').append(masterIncidentTemplate(id, description, km, im, time));
    jQuery("abbr.timeago").timeago();
}

var addMasterIncidents = function (incidents) {
    for (var i = 0; i < incidents.length; i++) {
        addMasterIncident(incidents[i]["MasterIncidentID"], incidents[i]["Description"], incidents[i]["KM"], incidents[i]["IM"], incidents[i]["Timestamp"]);
    }
}

var removeMasterIncident = function (id) {
    swal({
        title: 'Weet je zeker dat je dit incident wil verwijderen?',
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Verwijder',
        cancelButtonText: 'Laat maar'
    }).then(function () {
        sendRemoveMasterIncident(id);
        swal(
            'Klaar!',
            'Incident verwijderd',
            'success'
            )
    })
}

var deleteMasterIncident = function (incidentID) {
    $('#masterincident_' + incidentID).fadeOut(1000);
}


var masterIncidentTemplate = function (id, description, km, im, time) {
    var incident = "";
    incident = incident.concat('<div class="incident well" id="masterincident_' + id + '">');
    incident = incident.concat('<div class="incident-description">');
    incident = incident.concat(description);
    incident = incident.concat('</div>');
    incident = incident.concat('<div class="incident-KM">Gebruik: ' + km + ' </div>');
    incident = incident.concat('<div class="incident-IM">Relateer: ' + im + ' </div>');
    incident = incident.concat('<div class="incident-time"><i class="fa fa-clock-o" aria-hidden="true"></i><abbr class="timeago" title="' + time + '">' + time + ' </abbr>');

    if (userRole != "User") {
        incident = incident.concat('<i class="fa fa-trash" aria-hidden="true" onclick="removeMasterIncident(' + id + ');"></i>');
    }

    incident = incident.concat('</div>');
    incident = incident.concat('</div>');
    return incident;
}