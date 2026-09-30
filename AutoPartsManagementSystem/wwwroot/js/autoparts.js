/* AutoParts — wwwroot/js/autoparts.js  (Bootstrap 5 bundle kerak, jQuery shart emas) */
(function () {
  'use strict';
  const $ = (s, r = document) => r.querySelector(s);
  const $$ = (s, r = document) => Array.from(r.querySelectorAll(s));
  const fmt = n => new Intl.NumberFormat('en-US').format(Math.round(n));

  /* ---------- Toast:  toast('Saved')  yoki  data-toast="Saved" ---------- */
window.toast = function (msg, type) {
    const el = $('#toast');
    if (!el || !window.bootstrap) return;
    $('.toast-body', el).textContent = msg;
    el.classList.toggle('error', type === 'error');
    bootstrap.Toast.getOrCreateInstance(el, { delay: 2500 }).show();
};
  document.addEventListener('click', e => {
    const t = e.target.closest('[data-toast]');
    if (t) { e.preventDefault(); toast(t.dataset.toast); }
  });
  // Controller'dan: TempData["Toast"] = "Product saved";  -> <body data-flash="...">
    if (document.body.dataset.flash) toast(document.body.dataset.flash, document.body.dataset.flashType);

  /* ---------- Sidebar (mobil) ---------- */
  const menu = $('#menu');
  if (menu) menu.addEventListener('click', () => document.body.classList.toggle('side-open'));
  const overlay = $('.side-overlay');
  if (overlay) overlay.addEventListener('click', () => document.body.classList.remove('side-open'));

  /* ---------- Jadval filtri ---------- */
  function filterTables(tables, q) {
    q = q.trim().toLowerCase();
    tables.forEach(t => $$('tr', t).forEach(tr => {
      if (!tr.querySelector('td')) return;            // sarlavha qatori
      tr.style.display = tr.textContent.toLowerCase().includes(q) ? '' : 'none';
    }));
  }
  // Header'dagi global qidiruv — sahifadagi barcha jadvallarni filtrlaydi
  const gs = $('#globalSearch');
  if (gs) gs.addEventListener('input', () => filterTables($$('.content table'), gs.value));
  // Lokal qidiruv: <input data-filter="#productTable">
  $$('[data-filter]').forEach(inp => inp.addEventListener('input', () =>
    filterTables($$(inp.dataset.filter), inp.value)));

  /* ---------- O'chirishni tasdiqlash: <form data-confirm="Delete X?"> ---------- */
  document.addEventListener('submit', e => {
    const f = e.target;
    if (f.dataset.confirm && !confirm(f.dataset.confirm)) e.preventDefault();
  });

  /* ---------- Tabs: <button data-tab="sales"> + <div data-pane="sales"> ---------- */
  $$('[data-tabs]').forEach(group => {
    const scope = group.closest('[data-tabs-scope]') || document;
    group.addEventListener('click', e => {
      const b = e.target.closest('[data-tab]');
      if (!b) return;
      $$('[data-tab]', group).forEach(x => x.classList.toggle('active', x === b));
      $$('[data-pane]', scope).forEach(p => p.classList.toggle('show', p.dataset.pane === b.dataset.tab));
    });
  });

  /* ---------- Parolni ko'rsatish ---------- */
  $$('.pw button').forEach(b => b.addEventListener('click', () => {
    const i = b.parentElement.querySelector('input');
    const show = i.type === 'password';
    i.type = show ? 'text' : 'password';
    b.textContent = show ? 'Hide' : 'Show';
  }));

  /* ---------- Fayl tanlanganda nomini ko'rsatish ---------- */
  $$('.upload input[type=file]').forEach(inp => inp.addEventListener('change', () => {
    const span = inp.parentElement.querySelector('span');
    if (inp.files.length && span) span.textContent = inp.files[0].name;
  }));

  /* ---------- POS (Sales/Index) ---------- */
  const cartList = $('#cartList');
  if (cartList) {
    let cart = [];
    const discount = $('#discount');

    const render = () => {
      cartList.innerHTML = cart.length ? cart.map((i, idx) => `
        <div class="cart-item">
          <div class="top"><span>${i.name}</span><button type="button" data-remove="${i.id}" aria-label="Remove">✕</button></div>
          <div class="row2">
            <span class="qty"><button type="button" data-dec="${i.id}">−</button>${i.qty}<button type="button" data-inc="${i.id}">+</button></span>
            <span class="num">${fmt(i.price * i.qty)}</span>
          </div>
          <input type="hidden" name="Items[${idx}].ProductId" value="${i.id}">
          <input type="hidden" name="Items[${idx}].Quantity" value="${i.qty}">
        </div>`).join('')
        : '<div class="cart-empty">Cart is empty.<br>Tap a product to add it.</div>';

      const qty = cart.reduce((s, i) => s + i.qty, 0);
      const sub = cart.reduce((s, i) => s + i.qty * i.price, 0);
      const d = Math.min(100, Math.max(0, +discount.value || 0));
      $('#cartCount').textContent = qty + (qty === 1 ? ' item' : ' items');
      $('#subtotal').textContent = fmt(sub) + ' UZS';
      $('#total').textContent = fmt(sub * (1 - d / 100)) + ' UZS';
      $('#checkout').disabled = !cart.length;
    };

    $$('.tile[data-id]').forEach(t => t.addEventListener('click', () => {
      const id = +t.dataset.id, ex = cart.find(c => c.id === id);
      if (ex) ex.qty++;
      else cart.push({ id, name: t.dataset.name, price: +t.dataset.price, qty: 1 });
      render();
    }));
    cartList.addEventListener('click', e => {
      const b = e.target.closest('button'); if (!b) return;
      const id = +(b.dataset.inc || b.dataset.dec || b.dataset.remove);
      const it = cart.find(c => c.id === id);
      if (b.dataset.inc) it.qty++;
      if (b.dataset.dec) it.qty = Math.max(1, it.qty - 1);
      if (b.dataset.remove) cart = cart.filter(c => c.id !== id);
      render();
    });
    discount.addEventListener('input', render);
    render();
  }
})();
