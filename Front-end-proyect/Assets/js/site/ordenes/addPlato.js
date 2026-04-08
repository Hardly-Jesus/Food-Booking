const containerInputPlatoPedido = document.getElementById("containerInputPlatoPedido");
const formAddPlato = document.getElementById("form-plato-add-to-orden");



const params = new URLSearchParams(window.location.search);
const PedidoId = Number(params.get("id"));
const propietarioId = params.get("propietarioId");




async function getPlatosByMenu(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/Plato/get-Plato-not-add-pedido/${propietarioId}/${PedidoId}`,{
            headers: {
               
                Authorization: `Bearer ${token}`
                
            }
        });
          
    
        renderPlatos(result.data);
    
    }catch(err){

        console.log("Error al obtener los platos del menu", err);
    }
}



function renderPlatos(data) {

  const usuarioId = propietarioId;  
  containerInputPlatoPedido.innerHTML = "";
   
  if (!data || data.length === 0) {
    containerInputPlatoPedido.innerHTML = `
      <div class="col-12 text-center mt-4">
        <p class="text-muted"> Aún no hay platos  registrado en el menu, favor verficar antes de realizar el pedido 🍽️</p>
         <div class="mt-3 mb-3">
           <a href="/Assets/view/cliente/restaurante/menu.html?id=${usuarioId}">
             <button class="btn btn-outline-danger btn-sm">
               <i class="bi bi-eye-fill"></i> ver menu
            </button>
           </a>
            </div>
      </div>
    `;
    return;
  }

  

  data.forEach(p => {

    containerInputPlatoPedido.innerHTML += `
      <div class="form-check mb-3">
        <input class="form-check-input" type="checkbox" name="IdPlatos[]" value="${p.id}">
        <label class="form-check-label fw-bold">
          ${p.nombre} - RD$ ${p.precio}
        </label>

        <div class="mb-3">
          <label for="cantidadPlatos" class="form-label">Cantidad de platos</label>
          <input type="number" id="cantidadPlatos-${p.id}" name="cantidadPlatos" class="form-control">
        </div>
      </div>   

    `;
  });
    
}


document.addEventListener("DOMContentLoaded", () => {
    getPlatosByMenu();
});



formAddPlato.addEventListener("submit",function(e){
 e.preventDefault();
   AddPlatoPedido();
});



async function AddPlatoPedido(){
    try {
        const inputs = document.querySelectorAll("input[name='IdPlatos[]']:checked");
        const token = localStorage.getItem("token");

        const data = Array.from(inputs).map(input => {
            const idPlato = parseInt(input.value);

            const cantidadInput = document.getElementById(`cantidadPlatos-${idPlato}`);
            const cantidad = parseInt(cantidadInput.value) || 0;

            return {
                IdPedido: parseInt(id), 
                IdPlato: idPlato,
                CantidadPlatos: cantidad
            };
        });

        console.log(data); 

        const result = await axios.post(
            `${config.API_URL}/Api/v1/PedidoPlato/Add-platos-all-pedido`,
            data,
            {
                headers:{
                    Authorization: `Bearer ${token}`
                }
            }
        );

        window.location.href = "/Assets/view/cliente/ordenes/Ordenes.html";

    } catch(err) {
        console.log("Error: " + err);
    }
}