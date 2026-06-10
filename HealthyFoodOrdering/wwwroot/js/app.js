const menuItems = [

  {
    id:1,
    name:"Garden Salad Bowl",
    price:9.50,
    calories:320,
    time:"10 min",
    image:"https://images.unsplash.com/photo-1512621776951-a57141f2eefd?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  },

  {
    id:2,
    name:"Rainbow Protein Bowl",
    price:11.00,
    calories:450,
    time:"12 min",
    image:"https://images.unsplash.com/photo-1546069901-ba9599a7e63c?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  },

  {
    id:3,
    name:"Avocado Toast",
    price:7.50,
    calories:280,
    time:"8 min",
    image:"https://images.unsplash.com/photo-1525351484163-7529414344d8?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  },

  {
    id:4,
    name:"Berry Smoothie Bowl",
    price:8.50,
    calories:290,
    time:"8 min",
    image:"https://images.unsplash.com/photo-1610441009633-b6ca9c6d4be2?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  },

  {
    id:5,
    name:"Chicken Caesar Wrap",
    price:10.20,
    calories:410,
    time:"11 min",
    image:"https://images.unsplash.com/photo-1608039755401-742074f0548d?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  },

  {
    id:6,
    name:"Green Detox Smoothie",
    price:6.90,
    calories:180,
    time:"5 min",
    image:"https://images.unsplash.com/photo-1623065422902-30a2d299bbe4?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  },

  {
    id:7,
    name:"Vegan Buddha Bowl",
    price:12.50,
    calories:520,
    time:"14 min",
    image:"https://images.unsplash.com/photo-1547592180-85f173990554?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  },

  {
    id:8,
    name:"Healthy Pancakes",
    price:8.90,
    calories:350,
    time:"9 min",
    image:"https://images.unsplash.com/photo-1528207776546-365bb710ee93?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  },

  {
    id:9,
    name:"Fresh Fruit Bowl",
    price:7.20,
    calories:210,
    time:"6 min",
    image:"https://images.unsplash.com/photo-1490474418585-ba9bad8fd0ea?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  },

  {
    id:10,
    name:"Healthy Sandwich",
    price:9.90,
    calories:390,
    time:"10 min",
    image:"https://images.unsplash.com/photo-1528735602780-2552fd46c7af?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  },

  {
    id:11,
    name:"Protein Oatmeal",
    price:6.80,
    calories:260,
    time:"7 min",
    image:"https://images.unsplash.com/photo-1517673400267-0251440c45dc?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  },

  {
    id:12,
    name:"Detox Juice",
    price:5.90,
    calories:120,
    time:"4 min",
    image:"https://images.unsplash.com/photo-1622484212850-eb596d769edc?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400"
  }

]; 



let cart = [];

const menuGrid = document.getElementById("menuGrid");



menuItems.forEach(item => {

  menuGrid.innerHTML += `

    <div class="food-card">

      <img src="${item.image}" alt="${item.name}">

      <h3>${item.name}</h3>

      <div class="food-meta">
        <span>${item.calories} cal</span>
        <span>${item.time}</span>
      </div>

      <h4>$${item.price.toFixed(2)}</h4>

      <button id="btn-${item.id}" onclick="addToCart(${item.id})">
        Add To Cart
      </button>

    </div>

  `;

}); 

@model IEnumerable < Product >

    <div class="menu-grid">

        @foreach(var item in Model)
        {
            <div class="food-card">

                <img src="@item.ImageUrl">

                    <h3>@item.Name</h3>

                    <div class="food-meta">
                        <span>@item.Calories cal</span>
                        <span>@item.PreparationTime</span>
                    </div>

                    <h4>@item.FinalPrice.ToString("N0") VNĐ</h4>

                    <a asp-controller="Cart"
                        asp-action="AddProduct"
                        asp-route-id="@item.Id"
                        class="btn btn-success">
                        Thêm giỏ hàng
                    </a>

            </div>
        }

    </div>

function addToCart(id){

  const item = menuItems.find(food => food.id === id);

  cart.push(item);

  document.getElementById("cartCount").innerText = cart.length;

  renderCart();

  const btn = document.getElementById(`btn-${id}`);

  btn.innerText = "Added ✓";

  btn.style.background = "#2e5c3a";

  setTimeout(() => {

    btn.innerText = "Add To Cart";

    btn.style.background = "#4a7c59";

  },1000);

}



function renderCart(){

  const cartItems = document.getElementById("cartItems");

  cartItems.innerHTML = "";

  let total = 0;

  cart.forEach(item => {

    total += item.price;

    cartItems.innerHTML += `

      <div class="cart-item">

        <h4>${item.name}</h4>

        <p>$${item.price.toFixed(2)}</p>

      </div>

    `;

  });

  document.getElementById("cartTotal").innerText = total.toFixed(2);

}



function toggleCart(){

  document.getElementById("cartPanel")
  .classList.toggle("active");

}



document
.getElementById("cartButton")
.addEventListener("click",toggleCart);



function scrollToMenu(){

  document.getElementById("menu")
  .scrollIntoView({
    behavior:"smooth"
  });

}