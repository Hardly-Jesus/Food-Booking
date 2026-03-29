
const containerRestaurante = document.getElementById("RestauranteContainer");


async function GetRestaurante(){

try
{
    const token = localStorage.getItem("token");
    const result = await axios.get("https://localhost:7039/Api/v1/Restaurante/GetAll-restaurante",{
       headers: {
           Authorization: `Bearer ${token}`
       }
    });

    renderRestaurante(result.data);
}
catch(err)
{
     console.log("Ocurrio un error al obtener los restaurante" + err);
}
}
    



function renderRestaurante(data) {

  containerRestaurante.innerHTML = "";

data.forEach(r => {

const card = `
  <div class="col-md-10 col-lg-10 mt-4 me-3">
    <div class="card border-0 shadow-sm h-100 rounded-4 overflow-hidden">

      <!-- Imagen -->
      <div style="height: 200px; overflow: hidden;">
        <img src="https://localhost:7039/${r.imagen}" 
             class="w-100 h-100" 
             style="object-fit: cover; transition: transform .3s;">
      </div>

      <!-- Body -->
      <div class="card-body d-flex flex-column">

        <!-- Título -->
        <h5 class="fw-bold mb-2 text-primary">
          ${r.nombre}
        </h5>

        <!-- Info -->
        <p class="mb-1 text-muted small">
          <i class="bi bi-geo-alt-fill text-danger"></i> ${r.direccion}
        </p>

        <p class="mb-1 text-muted small">
          <i class="bi bi-telephone-fill text-success"></i> ${r.telefono}
        </p>

        <p class="mb-2 text-muted small">
          <i class="bi bi-clock-fill text-warning"></i> 
          ${r.horarioInicio} - ${r.horarioFin}
        </p>

        <!-- Especialidad -->
        <span class="badge bg-dark mb-3">
          ${r.especialidadGastronomica}
        </span>

        
      
        <!-- Botones -->
        <div class="mt-auto d-flex justify-content-between">
        <div class="mb-3">
         <button class="btn btn-outline-warning btn-sm px-3"> 
         <a href="/Assets/view/cliente/reserva/add.html?id=${r.usuarioId}&idRestaurante=${r.id}" class="text-decoration-none">     
           <i class="bi bi-calendar-plus"></i> Add Reserva
          </a>
        </button>
        </div>

        <div class="mb-3">
         <a href="/Assets/view/cliente/reseña/add.html class="text-decoration-none"> 
          <button class="btn btn-outline-danger btn-sm px-3">
            <i class="bi bi-chat-square-dots-fill"></i> Add Reseña
          </button>
          </a>
        </div>

          <div class="mb-3">
          <a href="/Assets/view/Propietario/menu/menu.html class="text-decoration-none"> 
          <button class="btn btn-outline-danger btn-sm px-3">
            <i class="bi bi-journal-text"></i> Ver menu
          </button>
          </a>
          </div>

          <div class="mb-3">
          <a href="/Assets/view/cliente/ordenes/add.html class="text-decoration-none"> 
          <button class="btn btn-outline-danger btn-sm px-3">
           <i class="bi bi-cart-check"></i> Realizar pedido
          </button>
          </a>
          </div>

        </div>


        <div id="containerReseña" class="mt-3">

        </div>

      </div>
    </div>
  </div>
`;

    containerRestaurante.innerHTML += card;
  });
}





document.addEventListener("DOMContentLoaded", () => {
    GetRestaurante();
});


