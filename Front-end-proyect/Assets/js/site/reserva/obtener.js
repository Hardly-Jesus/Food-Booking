const containerReserva = document.getElementById("ReservaContainer");


async function getReserva(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get("https://localhost:7039/Api/v1/Reserva/get-all-By-UsuarioId",{
            headers: {
               
                Authorization: `Bearer ${token}`
                
            }
        });
          
    
        renderReservas(result.data);
    
    }catch(err){

        console.log("Error al obtener restaurante", err);
    }
}




function renderReservas(data) {

  containerReserva.innerHTML = "";

  data.forEach(r => {
   const card = `
  <div class="col-md-6 col-lg-4 mt-4">
    <div class="card border-0 shadow-sm h-100 rounded-4 overflow-hidden">

      <div class="bg-light px-3 py-2 border-bottom">
        <h6 class="fw-bold text-primary mb-0">
          <i class="bi bi-calendar-event"></i>
          ${r.fecha} <br> 
          <i class="bi bi-hourglass-split"></i> Hora: ${r.hora}
        </h6>
      </div>

  
      <div class="card-body d-flex flex-column">

        <!-- Restaurante -->
        <p class="mb-2 text-dark">
          <i class="bi bi-shop text-success me-1"></i>
          <span class="fw-semibold">${r.restaurante}</span>
        </p>

   
        <p class="mb-3 text-muted small">
          <i class="bi bi-grid-3x3-gap-fill text-warning me-1"></i>
          Mesa: <span class="fw-semibold text-dark">${r.mesa}</span>
        </p>

   
        <span class="badge bg-dark text-warning mb-3 align-self-start">
          ${r.estado}
        </span>

    
        <div class="mt-auto d-flex justify-content-between">
          
          <a href="/Assets/view/cliente/reserva/edit.html?id=${r.id}&propietarioId=${r.propietarioId}"
             class="btn btn-outline-warning btn-sm px-3 rounded-pill">
            <i class="bi bi-pencil"></i>
          </a>

          <a href="/Assets/view/cliente/reserva/delete.html?id=${r.id}" 
             class="btn btn-outline-danger btn-sm px-3 rounded-pill">
            <i class="bi bi-trash"></i>
          </a>

        </div>
      </div>
    </div>
   </div>
  `;

    containerReserva.innerHTML += card;
  });
}



document.addEventListener("DOMContentLoaded", () => {
    getReserva();
});