const containerIndciadores = document.getElementById("IndicadodoresContainer");


async function getIndicadoresDasboard(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/Plato/get-indicadores`,{
            headers: {
               
                Authorization: `Bearer ${token}`    
            }
        });
          
      
        renderDasboard(result.data);
    
    }catch(err){

        console.log("Error al obtener los indicadores del dasboard", err);
    }
}



function renderDasboard(data) {

  if (!data) {
    containerIndciadores.innerHTML = "No se pudieron encontrar los indicadores para el dasboard";
    return;
  }

  containerIndciadores.innerHTML = `
    <div class="row g-4 mt-3">

      <!-- Reservas -->
      <div class="col-12 col-sm-6 col-lg-3">
        <div class="card shadow-sm border-0 rounded-4 text-center p-3 h-100">
          <div class="mb-2 text-primary fs-2">
            <i class="bi bi-calendar-check"></i>
          </div>
          <h6 class="text-muted">Total Reservas</h6>
          <h2 class="fw-bold">${data.totalReserva}</h2>
        </div>
      </div>

      <!-- Pedidos -->
      <div class="col-12 col-sm-6 col-lg-3">
        <div class="card shadow-sm border-0 rounded-4 text-center p-3 h-100">
          <div class="mb-2 text-success fs-2">
            <i class="bi bi-bag-check"></i>
          </div>
          <h6 class="text-muted">Total Pedidos</h6>
          <h2 class="fw-bold">${data.totalPedido}</h2>
        </div>
      </div>

      <!-- Pagos -->
      <div class="col-12 col-sm-6 col-lg-3">
        <div class="card shadow-sm border-0 rounded-4 text-center p-3 h-100">
          <div class="mb-2 text-warning fs-2">
            <i class="bi bi-credit-card"></i>
          </div>
          <h6 class="text-muted">Pagos Procesados</h6>
          <h2 class="fw-bold">${data.totalPagoProcesado}</h2>
        </div>
      </div>

      <!-- Reseñas -->
      <div class="col-12 col-sm-6 col-lg-3">
        <div class="card shadow-sm border-0 rounded-4 text-center p-3 h-100">
          <div class="mb-2 text-danger fs-2">
            <i class="bi bi-star-fill"></i>
          </div>
          <h6 class="text-muted">Reseñas</h6>
          <h2 class="fw-bold">${data.totalResenia}</h2>
        </div>
      </div>

    </div>
  `;
}



document.addEventListener("DOMContentLoaded", () => {
   getIndicadoresDasboard();
});
