var productTable;

window.initProductTable = function () {

    if ($.fn.DataTable.isDataTable('#tableProduct')) return;

    productTable = $('#tableProduct').DataTable({
        ajax: { url: '/admin/product/getall' },
        columns: [
            { data: 'title' },
            { data: 'isbn' },
            { data: 'listPrice' },
            { data: 'author' },
            { data: 'category.name' },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <div class="btn-group">
                            <button onclick="openAdminModal('/admin/product/upsert/${data}')" 
                                    class="btn btn-success btn-sm mx-1">
                                <i class="bi bi-pencil-square"></i>
                            </button>
                            <button onclick="deleteProduct('/admin/product/delete/${data}')" 
                                    class="btn btn-danger btn-sm mx-1">
                                <i class="bi bi-trash-fill"></i>
                            </button>
                        </div>`;
                }
            }
        ],
        columnDefs: [
            { targets: -1, orderable: false, className: "text-center", width: "140px" }
        ]
    });
};

function deleteProduct(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: 'This product will be deleted!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    productTable.ajax.reload(null, false);
                    toastr.success(data.message);
                }
            });
        }
    });
}
function submitProductForm() {
    var form = $("#productForm")[0];
    var formData = new FormData(form);

    $.ajax({
        url: "/admin/product/upsert",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.success) {
                $("#adminModal").modal("hide");
                productTable.ajax.reload(null, false);
                toastr.success("Product saved successfully");
            } 
            else {
                $("#adminModal .modal-content").html(res);
            }
        },
        error: function () {
            toastr.error("Something went wrong. Please try again.");
        }
    });
}