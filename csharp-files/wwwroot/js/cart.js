// Cart functionality for product detail pages
let cartId = null;

// Initialize cart
async function initCart() {
    try {
        // Try to get existing cart from session
        const response = await fetch('/api/v1/cart/session', {
            credentials: 'include'
        });

        if (response.ok) {
            const cart = await response.json();
            cartId = cart.cartId;
        } else {
            // Create new cart
            const createResponse = await fetch('/api/v1/cart', {
                method: 'POST',
                credentials: 'include'
            });
            const newCart = await createResponse.json();
            cartId = newCart.cartId;
        }

        updateCartCount();
    } catch (error) {
        console.error('Error initializing cart:', error);
    }
}

// Add to cart
async function addToCart(variantId, quantity = 1) {
    if (!cartId) {
        await initCart();
    }

    try {
        const response = await fetch(`/api/v1/cart/${cartId}/items`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            body: JSON.stringify({ variantId, quantity })
        });

        if (response.ok) {
            const cart = await response.json();
            showNotification('Đã thêm vào giỏ hàng!', 'success');
            updateCartCount();
            return true;
        } else {
            showNotification('Có lỗi khi thêm vào giỏ hàng', 'error');
            return false;
        }
    } catch (error) {
        console.error('Error adding to cart:', error);
        showNotification('Có lỗi khi thêm vào giỏ hàng', 'error');
        return false;
    }
}

// Update cart count in navbar
async function updateCartCount() {
    if (!cartId) return;

    try {
        const response = await fetch(`/api/v1/cart/${cartId}`, {
            credentials: 'include'
        });

        if (response.ok) {
            const cart = await response.json();
            const count = cart.items.reduce((sum, item) => sum + item.quantity, 0);
            
            const cartBadge = document.querySelector('.cart-count');
            if (cartBadge) {
                cartBadge.textContent = count;
                cartBadge.style.display = count > 0 ? 'block' : 'none';
            }
        }
    } catch (error) {
        console.error('Error updating cart count:', error);
    }
}

// Show notification
function showNotification(message, type = 'success') {
    const notification = document.createElement('div');
    notification.className = `fixed top-4 right-4 px-6 py-3 rounded-lg shadow-lg text-white z-50 ${
        type === 'success' ? 'bg-green-500' : 'bg-red-500'
    }`;
    notification.textContent = message;
    
    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, 3000);
}

// Initialize cart on page load
document.addEventListener('DOMContentLoaded', () => {
    initCart();

    // Setup add to cart buttons
    document.querySelectorAll('.add-to-cart-btn').forEach(btn => {
        btn.addEventListener('click', async function(e) {
            e.preventDefault();
            
            const variantId = this.dataset.variantId;
            const quantityInput = this.closest('form')?.querySelector('input[name="quantity"]');
            const quantity = quantityInput ? parseInt(quantityInput.value) : 1;

            if (!variantId) {
                showNotification('Vui lòng chọn size và màu sắc', 'error');
                return;
            }

            const success = await addToCart(parseInt(variantId), quantity);
            
            if (success) {
                // Optional: show mini cart or redirect to cart page
            }
        });
    });
});
