const containerInputPlato = document.getElementById("containerInputPlato");
const formAddPlato = document.getElementById("form-add-platoMenu");



const params = new URLSearchParams(window.location.search);
const id = params.get("id");


async function getPlatosByUser(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/Plato/get-Plato-not-add-Menu`,{
            headers: {
               
                Authorization: `Bearer ${token}`
                
            }
        });
          
    
        renderPlatos(result.data);
    
    }catch(err){

        console.log("Error al obtener restaurante", err);
    }
}



function renderPlatos(data) {

  containerInputPlato.innerHTML = "";
   
  if (!data || data.length === 0) {
    containerInputPlato.innerHTML = `
      <div class="col-12 text-center mt-4">
        <p class="text-muted"> Aún no hay platos  registrado, favor iniciar a registrar 🍽️</p>
         <div class="mt-3 mb-3">
           <a href="/Assets/view/Propietario/plato/plato.html">
             <button class="btn btn-outline-danger btn-sm">
                <i class="bi bi-bookmark-plus-fill"></i>  add Plato
            </button>
           </a>
            </div>
      </div>
    `;
    return;
  }

  
  data.forEach(p => {

    containerInputPlato.innerHTML += `
      <div class="form-check mb-3">
        <input class="form-check-input" type="checkbox" name="IdPlatos[]" value="${p.id}">
        <label class="form-check-label fw-bold">
          ${p.nombre} - RD$ ${p.precio}
        </label>
      </div>   

    `;
  });
    
}


document.addEventListener("DOMContentLoaded", () => {
    getPlatosByUser();
});



formAddPlato.addEventListener("submit",function(e){
 e.preventDefault();
 AddPlatoMenu();
});



async function AddPlatoMenu(){
   
    try
    {
        const inputs = document.querySelectorAll("input[name='IdPlatos[]']:checked");

        const ids = Array.from(inputs).map(i => parseInt(i.value));
        const idMenu = id;
        const IdPlatos = ids;
        const token = localStorage.getItem("token");

        const result = await axios.post(`${config.API_URL}/Api/v1/PlatoMenu/Add-plato-all-menu`,{idMenu,IdPlatos},{
            headers:{
                Authorization: `Bearer ${token}`
            }
        });
   
       
        return window.location.href = "/Assets/view/Propietario/menu/menu.html";

    }
    catch(err)
    {
        console.log("Ocurrio un error al intentar agregar el plato al menu" + err);
    }

}


