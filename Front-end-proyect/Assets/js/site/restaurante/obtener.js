
const container = document.getElementById("RestauranteContainer");
const btnAddRestaurante = document.getElementById("btn-add-restaurante");



async function getRestauranteByUsuarioId(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get("https://localhost:7039/Api/v1/Restaurante/GetByUserId",{
            headers: {
               
                Authorization: `Bearer ${token}`
                
            }
        });
          
    
        renderRestaurante(result.data);
    
    }catch(err){

        console.log("Error al obtener restaurante", err);
    }
}



function renderRestaurante(data) {

  container.innerHTML = "";

    const lista = Array.isArray(data) ? data : [data];

    if (lista.length > 0) {
           btnAddRestaurante.style.display = "none";
     }


  lista.forEach(r => {

    const card = `
  <div class="col-md-6 col-lg-6 mt-4">
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
        <button class="btn btn-outline-warning btn-sm px-3"> 
         <a href="/Assets/view/Propietario/restaurante/edit.html?id=${r.id}" class="text-decoration-none">     
            <i class="bi bi-pencil"></i>
          </a>
        </button>


         <a href="/Assets/view/Propietario/restaurante/delete.html?id=${r.id}" class="text-decoration-none"> 
          <button class="btn btn-outline-danger btn-sm px-3">
            <i class="bi bi-trash"></i>
          </button>
          </a>

        </div>

      </div>
    </div>
  </div>
`;

    container.innerHTML += card;
  });
}





document.addEventListener("DOMContentLoaded", () => {
    getRestauranteByUsuarioId();
});