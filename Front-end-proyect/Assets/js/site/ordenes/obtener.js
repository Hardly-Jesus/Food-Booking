const ordenesContainer = document.getElementById("ordenesContainer");
const platoOrdenesContainer = document.getElementById("platos-reserva-container");
const usuarioId = localStorage.getItem("UsuarioId");



async function GetPedidosUsuario()
{
    try{
      
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/Pedido/get-All-pedidos/${usuarioId}`,{
           headers: {
            Authorization: `Bearer ${token}`
           } 
        });


          reenderizarPedidos(result.data);


    }catch(err){

       console.log("Ocurrio un error al intentar obtener los pedidos" + err);

    }
}




async function reenderizarPedidos(data) {
 
  ordenesContainer.innerHTML = "";
  
  data.forEach(r => {
   const card = `
  <div class="col-md-10 col-lg-10 mt-4">
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


         <p class="mb-3 text-muted small">
          <i class="bi bi-currency-dollar"></i>
          total: <span class="fw-semibold text-dark">${r.total}</span>
        </p>

   
        <span class="badge bg-dark text-warning mb-3 align-self-start">
          ${r.estado}
        </span>

        <div class="mt-auto d-flex justify-content-between">
          
          <a href="/Assets/view/cliente/ordenes/edit.html?id=${r.id}"
             class="btn btn-outline-warning btn-sm px-3 rounded-pill">
            <i class="bi bi-pencil"></i> Edit 
          </a>

          <a href="/Assets/view/cliente/ordenes/delete.html?id=${r.id}" 
             class="btn btn-outline-danger btn-sm px-3 rounded-pill">
            <i class="bi bi-trash"></i> Delete
          </a>

           <a href="/Assets/view/cliente/ordenes/pago.html?id=${r.id}" 
             class="btn btn-outline-success btn-sm px-3 rounded-pill">
              <i class="bi bi-paypal"></i>  Pagar
          </a>
    

          <a href="/Assets/view/cliente/ordenes/addPlato.html?id=${r.id}&propietarioId=${r.propietarioId}" 
             class="btn btn-outline-primary btn-sm px-3 rounded-pill">
              <i class="bi bi-fork-knife"></i> Add Plato
          </a>
        </div>
          
         
        <div id="platos-pedido-container-${r.id}" class="mb-3 mt-3">
             
        </div>

      </div>
    </div>
   </div>
   

  `;
   
   
    ordenesContainer.innerHTML += card;
  });
  

   for (const r of data) {
    await GetPlatosPedidos(r.id);
   }

  
}   




async function GetPlatosPedidos(pedidoId){
 
     try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/Plato/get-Platos-pedido/${pedidoId}`,{
            headers: {
               
                Authorization: `Bearer ${token}`    
            }
        });
          
      
       renderPlatoInPedido(result.data,pedidoId);
    
    }catch(err){

        console.log("Error al obtener los platos", err);
    }


  }



  
function renderPlatoInPedido(data,PedidoId) {

  const platoPedidoContainer = document.getElementById(`platos-pedido-container-${PedidoId}`);

  platoPedidoContainer.innerHTML = "";

  if (!data || data.length === 0) {
    platoPedidoContainer.innerHTML = `
      <div class="col-12 text-center mt-4">
        <p class="text-muted">Aún no hay platos agregados al pedidos 🍽️</p>
      </div>
    `;
    return;
  }

  data.forEach(m => {
    platoPedidoContainer.innerHTML += `
      <div class="col-md-6 col-lg-6 mt-3">
        <div class="card shadow-sm">
          <div class="card-body bg-info">
            <h6 class="fw-bold text-center text-light">${m.nombre}</h6>
            <p class="small">${m.descripcion}</p>
            <p class="fw-bold">RD$ ${m.precio}</p>

            <div class="mt-3 mb-3">
           <a href="/Assets/view/Propietario/menu/deletePlatoMenu.html?idPlato=${m.id}&idMenu=${PedidoId}">
             <button class="btn btn-outline-danger btn-sm">
                      <i class="bi bi-trash"></i>
            </button>
           </a>
            </div>
          </div>
        </div>
      </div>
    `;
  });
}



document.addEventListener("DOMContentLoaded", () => {
     GetPedidosUsuario();
});

