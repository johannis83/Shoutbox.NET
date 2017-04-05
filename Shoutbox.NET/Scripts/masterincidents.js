var createMasterIncident = function () {
    swal({
        title: "Nieuw master incident toevoegen",
        text: "Geef een omschrijving",
        html:
        'Omschrijving: <input type="text"<br>' +
        'KM: <input type="text"<br>' +
        'IM: <input type="text"<br>',
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