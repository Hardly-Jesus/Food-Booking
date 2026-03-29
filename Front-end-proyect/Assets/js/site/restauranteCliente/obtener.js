
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
         <button 
            class="btn btn-outline-danger btn-sm px-3"
            onclick="toggleReseña(${r.id})">
            <i class="bi bi-chat-square-dots-fill"></i> Add Reseña
         </button>
         </div>

          <div class="mb-3">
          <a href="/Assets/view/cliente/restaurante/menu.html?id=${r.usuarioId}" class="text-decoration-none"> 
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

  
        <div class="mt-3 mb-3 d-none" id="form-add-resena-container-${r.id}">
       <div class="card border-0 shadow-sm rounded-4">

   
    <div class="card-header bg-white border-0">
      <h6 class="mb-0 fw-bold text-primary">
        <i class="bi bi-chat-left-text"></i> Escribir reseña
      </h6>
    </div>

   
    <div class="card-body">

      <form id="resena-add-form-${r.id}">

        <!-- Descripción -->
        <div class="mb-3">
          <label class="form-label fw-semibold small text-muted">
            Tu experiencia
          </label>

          <textarea 
            id="descripcion${r.id}" 
            class="form-control rounded-3 shadow-sm" 
            rows="3"
            placeholder="Ej: La comida estuvo excelente, el servicio fue rápido..."
            required>
          </textarea>
        </div>

      
        <div class="mb-3">
          <label class="form-label fw-semibold small text-muted">
            Calificación
          </label>

          <select 
            id="cantidadEstrella${r.id}" 
            class="form-select rounded-3 shadow-sm">
            <option value="5">⭐⭐⭐⭐⭐ Excelente</option>
            <option value="4">⭐⭐⭐⭐ Muy bueno</option>
            <option value="3">⭐⭐⭐ Bueno</option>
            <option value="2">⭐⭐ Regular</option>
            <option value="1">⭐ Malo</option>
          </select>
        </div>

        
        <div class="d-flex justify-content-end gap-2 mt-4">
          <button 
            type="button"
            class="btn btn-light border rounded-3 px-3"
            onclick="toggleReseña(${r.id})">
            Cancelar
          </button>
         

          <button 
            type="button" onclick="publicarReseña(${r.id})"
            class="btn btn-primary rounded-3 px-4 shadow-sm">
            <i class="bi bi-send"></i> Publicar
          </button>
        </div>
       </form>
      </div>
     </div>
     </div> 


        <div id="containerReseña-${r.id}" class="mt-3">

        </div>
      </div>
    </div>
  </div>
`;
      GetReseñaRestaurante(r.id);
    containerRestaurante.innerHTML += card;
  });
}



function toggleReseña(id) {
  const form = document.getElementById(`form-add-resena-container-${id}`);

  if (form.classList.contains("d-none")) {
    form.classList.remove("d-none");
  } else {
    form.classList.add("d-none");
  }
}


async function  publicarReseña(id){
  try{
          
      const descripcion = document.getElementById(`descripcion${id}`).value;
      const cantidadEstrella = Number(document.getElementById(`cantidadEstrella${id}`).value);
      const idRestaurante = id;
      const token = localStorage.getItem("token");
      const result = await axios.post("https://localhost:7039/Api/v1/Reseña/add-reseña",{descripcion,cantidadEstrella,idRestaurante},{
        headers:{
          Authorization: `Bearer ${token}`
        }
      });

  
     await GetReseñaRestaurante(id);      
   

  }catch(err){


      console.log("Ocurrio un error al intentar publicar la reseña" + err);

  }
}


async function GetReseñaRestaurante(id){

  try{
         const token = localStorage.getItem("token");
         const result = await axios.get(`https://localhost:7039/Api/v1/Reseña/get-all-resenia/${id}`,{
          headers: {
            Authorization: `Bearer ${token}`
          }
         })
     
        renderizarResenia(result.data,id);

  }catch(err){

       console.log("Ocurrio un error al intentar obtener las reseñas de este restaurante" + err);
  }
}


function renderizarResenia(data,id){

  const containerResenia = document.getElementById(`containerReseña-${id}`);

  if (!containerResenia) {
    console.error("No existe el contenedor de reseñas");
    return;
  }

  containerResenia.innerHTML = "";

  data.forEach(r => {
    const estrellas = "⭐".repeat(r.cantidadEstrella);
    const card = `
      <div class="card border-0 shadow-sm rounded-4 mb-3">
        <div class="card-body">

      
          <div class="mb-2 text-warning fs-5">
            ${estrellas}
          </div>

          <p class="mb-2 text-dark">
            ${r.descripcion}
          </p>

    
     
      <div class="d-flex justify-content-end gap-2 mt-3">
        <button 
          class="btn btn-sm btn-outline-warning"
          onclick="editarResena(${r.id}, ${id})">
          <i class="bi bi-pencil-square"></i> Editar
        </button>
         
        <a href="/Assets/view/cliente/reseña/delete.html?id=${r.id}" class="text-decoration-none"> 
        <button type="button"
          class="btn btn-sm btn-outline-danger">
          <i class="bi bi-trash"></i> Eliminar
        </button>
        </a>

      </div>
        </div>
      </div>
    `;

    containerResenia.innerHTML += card;
  });
}


async function editarResena(id){

}




document.addEventListener("DOMContentLoaded", () => {
    GetRestaurante();

});


