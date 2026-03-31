const container = document.getElementById("menuContainer");
const btnAddMenu = document.getElementById("btn-save-menu");
const platoMenuContainer = document.getElementById("platoMenuContainer");
const btnPlatoMenuContainer = document.getElementById("btn-actions-menu-plato");

async function getMenu(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`https://localhost:7039/Api/v1/Menu/get-menus-propietario`,{
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
      container.innerHTML = `${data.errors || "No se encontro el menu del restaurante"}`;
    return;
  }

  btnAddMenu.style.display = "none";
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

   
    <div id="btn-actions-menu-plato" class="mt-4 mb-5"></div>
  `;
}








async function GetPlatosMenu(){
 
     try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`https://localhost:7039/Api/v1/Plato/get-Platos-menu`,{
            headers: {
               
                Authorization: `Bearer ${token}`    
            }
        });
          
      
       return result.data;
    
    }catch(err){

        console.log("Error al obtener los platos del menu", err);
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

            <div class="mt-3 mb-3">
           <a href="/Assets/view/Propietario/menu/deletePlatoMenu.html?idPlato=${m.id}&idMenu=${menuId}">
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


function renderButtons(menuId) {

  const btnContainer = document.getElementById("btn-actions-menu-plato");

  btnContainer.innerHTML = `
    <div class="d-flex justify-content-between bg-secondary">

    <div class="mb-3 mt-3">
      <a href="/Assets/view/Propietario/menu/edit.html?id=${menuId}">
        <button class="btn btn-warning btn-sm ms-3">
          <i class="bi bi-pencil"></i> Edit Menu
        </button>
      </a>
    </div>

      <div class="mb-3 mt-3">
      <a href="/Assets/view/Propietario/menu/delete.html?id=${menuId}">
        <button class="btn btn-danger btn-sm">
          <i class="bi bi-trash"></i> Delete Menu
        </button>
      </a>
      </div>

      <div class="mb-3 mt-3">
      <a href="/Assets/view/Propietario/menu/addPlato.html?id=${menuId}">
        <button class="btn btn-primary btn-sm me-3">
          <i class="bi bi-fork-knife"></i> Add Platos
        </button>
      </a>
      </div>

    </div>
  `;
}



async function init() {

  const menu = await getMenu(); 
  renderMenu(menu);

  const platos = await GetPlatosMenu();
  renderPlatoInMenu(platos,menu.id);

  renderButtons(menu.id);
}






document.addEventListener("DOMContentLoaded", () => {
   init();
});


