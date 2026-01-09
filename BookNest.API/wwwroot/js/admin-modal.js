window.openAdminModal = function (url) {
    $("#adminModal").remove();

    $.get(url, function (html) {
        $("#modalContainer").html(`
            <div class="modal fade" id="adminModal" tabindex="-1">
                <div class="modal-dialog modal-xl modal-dialog-centered">
                    <div class="modal-content">${html}</div>
                </div>
            </div>
        `);

        new bootstrap.Modal(document.getElementById("adminModal")).show();
    });
};
