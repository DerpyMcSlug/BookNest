// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("click", function (e) {
    const button = e.target.closest(".btn");
    if (!button) return;

    const rect = button.getBoundingClientRect();
    const ripple = document.createElement("span");
    const size = Math.max(rect.width, rect.height);

    ripple.className = "ripple";
    ripple.style.width = ripple.style.height = size + "px";
    ripple.style.left = e.clientX - rect.left - size / 2 + "px";
    ripple.style.top = e.clientY - rect.top - size / 2 + "px";

    button.appendChild(ripple);

    ripple.addEventListener("animationend", () => {
        ripple.remove();
    });
});

document.addEventListener("click", function (e) {

    // ⭐ Wishlist
    const wishBtn = e.target.closest(".wishlist-btn");
    if (wishBtn) {
        e.preventDefault();

        wishBtn.disabled = true;
        setTimeout(() => wishBtn.disabled = false, 600);

        fetch('/Customer/Wishlist/Toggle?productId=' + wishBtn.dataset.productId, {
            method: 'POST'
        })
            .then(res => res.json())
            .then(data => {
                const icon = wishBtn.querySelector('i');

                icon.classList.toggle('bi-star', !data.added);
                icon.classList.toggle('bi-star-fill', data.added);
                wishBtn.classList.toggle('active', data.added);

                toastr[data.added ? "success" : "info"](data.message);
            });

        return;
    }

    // 🛒 Add to Cart
    const cartBtn = e.target.closest(".details-addcart");
    if (cartBtn) {
        e.preventDefault();

        const productId = cartBtn.dataset.productId;

        if (!productId) {
            toastr.error("Invalid product ID");
            return;
        }

        const returnUrl =
            document.querySelector('input[name="returnUrl"]')?.value
            || window.location.pathname + window.location.search;

        fetch('/Customer/Cart/AddAjax', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                productId: productId,
                count: 1,
                returnUrl: returnUrl
            })
        })
            .then(res => {
                if (!res.ok) throw new Error("Server error");
                return res.json();
            })
            .then(data => {
                if (data.success) {
                    toastr.success(data.message);
                } else {
                    toastr.error(data.message);
                }
            })
            .catch(() => {
                toastr.error("Something went wrong");
            });
    }
});