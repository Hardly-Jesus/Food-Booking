const container = document.getElementById("mesaContainer");




async function getMesas(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/Mesa/Get-mesas`,{
            headers: {
               
                Authorization: `Bearer ${token}`
                
            }
        });
          
      

        renderMesas(result.data);
    
    }catch(err){

        console.log("Error al obtener las mesas", err);
    }
}



function renderMesas(data) {

  container.innerHTML = "";

  data.forEach(r => {

    const card = `
<div class="col-md-4 col-lg-6 mt-4">
  <div class="card border-0 shadow-sm h-100 rounded-4 overflow-hidden">
    
    <div class="card-body d-flex flex-column">

      <!-- Título -->
      <h5 class="fw-bold mb-2 text-primary">
        ${r.nombre}
      </h5>

      <!-- Info -->
      <p class="mb-1 text-muted small">
        <i class="bi bi-geo-alt-fill text-danger"></i> ${r.descripcion}
      </p>

      <p class="mb-1 text-muted small">
        <i class="bi bi-people-fill text-success"></i> ${r.cantidadPersonas}
      </p>

      <!-- Botones -->
      <div class="mt-auto d-flex justify-content-between">

        <a href="/Assets/view/Propietario/mesa/edit.html?id=${r.id}" 
           class="btn btn-outline-warning btn-sm px-3">
          <i class="bi bi-pencil"></i>
        </a>

        <a href="/Assets/view/Propietario/mesa/delete.html?id=${r.id}" 
           class="btn btn-outline-danger btn-sm px-3">
          <i class="bi bi-trash"></i>
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
    getMesas();
});
