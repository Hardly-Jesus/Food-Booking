

const container = document.getElementById("menuContainer");
const platoMenuContainer = document.getElementById("platoMenuContainer");

const params = new URLSearchParams(window.location.search);
const usuarioId = params.get("id");



async function getMenu(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/Menu/get-menu/${usuarioId}`,{
            headers: {
               
                Authorization: `Bearer ${token}`    
            }
        });
          
      
        return result.data;
    
    }catch(err){

        console.log("Error al obtener el menu", err);
    }
}



function renderMenu(data) {

  if (data.hasErrors || data.nombre === null) {
      container.innerHTML = `${data.errors || "No se encontro un menu para este restaurante"}`;
    return;
  }

  container.innerHTML = `
    <div class="col-md-12 col-lg-12 mt-4">
      <div class="card border-0 shadow-sm rounded-4">
        <div class="card-body text-center bg-secondary">
          <h5 class="fw-bold text-primary">
            <i class="bi bi-card-heading"></i> ${data.nombre}
          </h5>

          <p class="text-muted small fw-bold">
            <i class="bi bi-text-paragraph"></i> ${data.descripcion}
          </p>
        </div>
      </div>
    </div>

    <div id="platoMenuContainer" class="row"></div>
    <div id="container-actions-volver" class="mb-3 mt-3">
    <div class="mb-3">
          <button type="button" class="btn btn-outline-warning">
            <a href="/Assets/view/cliente/restaurante/Restaurante.html" class="text-decoration-none">Back to list <i class="bi bi-skip-backward-btn-fill"></i></a>
          </button>
    </div
   </div>

  `;
}








async function GetPlatosMenu(){
 
     try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/Plato/get-Platos-menu/${usuarioId}`,{
            headers: {
               
                Authorization: `Bearer ${token}`    
            }
        });
          
      
       return result.data;
    
    }catch(err){

        console.log("Error al obtener el menu", err);
    }


  }



  
function renderPlatoInMenu(data,menuId) {

  const platoMenuContainer = document.getElementById("platoMenuContainer");

  platoMenuContainer.innerHTML = "";

  if (!data || data.length === 0) {
    platoMenuContainer.innerHTML = `
      <div class="col-12 text-center mt-4">
        <p class="text-muted">Aún no hay platos en el menú 🍽️</p>
      </div>
    `;
    return;
  }

  data.forEach(m => {
    platoMenuContainer.innerHTML += `
      <div class="col-md-6 col-lg-6 mt-3">
        <div class="card shadow-sm">
          <div class="card-body bg-info">
            <h6 class="fw-bold text-center text-light">${m.nombre}</h6>
            <p class="small">${m.descripcion}</p>
            <p class="fw-bold">RD$ ${m.precio}</p>
          </div>
        </div>
      </div>
    `;
  });
}


async function init() {

  const menu = await getMenu(); 
  renderMenu(menu);

  const platos = await GetPlatosMenu();
  renderPlatoInMenu(platos,menu.id);

}


document.addEventListener("DOMContentLoaded", () => {
   init();
});


