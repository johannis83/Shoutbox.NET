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
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        animation: "slide-from-top",
        inputPlaceholder: "Naam..."
    },
        function (inputValue) {
            if (inputValue === false) return false;

            if (inputValue === "") {
                swal.showInputError("Geef een naam op");
                return false
            }

            if (sendEditTeam(functie, inputValue) != null) {
                swal({
                    title: "Done!",
                    text: "De nieuwe " + functie + " van de dag is " + inputValue,
                    type: "success",
                    showCancelButton: false,
                });
            } else {
                swal({
                    title: "Fout!",
                    text: "Het wijzigen van de Team is niet gelukt, probeer de pagina te verversen.",
                    type: "error",
                    showCancelButton: false,
                });
            }
        });
}