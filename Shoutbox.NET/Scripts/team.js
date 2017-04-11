//Update a single Team
var updateTeam = function (functie, naam, changer) {
    $('#' + functie).html(naam);
    //Also set the title, so people can see who updated it
    $('#' + functie).parent().attr("title", "Geplaatst door " + changer);
}

//Update all Teams at once
var updateTeams = function (Teams) {
    for (var i = 0; i < Teams.length; i++) {
        updateTeam(Teams[i]["Functie"], Teams[i]["Naam"], Teams[i]["ModifiedBy"]);
    }
}

var editTeam = function (functie) {

    swal({
        title: functie + " van de dag wijzigen",
        text: "Geef de naam van de nieuwe " + functie + " van de dag op",
        input: "text",
        type: "info",
        showCancelButton: true,
        inputPlaceholder: "Naam...",
        confirmButtonText: "Prima!",
        confirmButtonColor: "#009",
    }).then(function (inputValue) {
        if (inputValue === false) return false;

        if (inputValue === "") {
            swal.showInputError("Geef een naam op");
            return false
        }

        //Tell server to change team
        sendEditTeam(functie, inputValue);

        //Notify user
        swal({
            title: "Done!",
            text: "De nieuwe " + functie + " van de dag is " + inputValue,
            type: "success",
            showCancelButton: false
        }).catch(swal.noop);
    }).catch(swal.noop);
}