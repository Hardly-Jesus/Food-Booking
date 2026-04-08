const containerUsuarios = document.getElementById("usuarioContainer");



async function getUsers(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/ManagerAccount/Get-Users`,{
            headers: {
               
                Authorization: `Bearer ${token}`
                
            }
        });
          
    
        renderUsuarios(result.data);
    
    }catch(err){

        console.log("Error al obtener restaurante", err);
    }
}



function renderUsuarios(data) {
  containerUsuarios.innerHTML = "";

  if (!data || data.length === 0) {
    containerUsuarios.innerHTML = `
      <div class="text-center text-muted mt-4">
        No hay usuarios disponibles
      </div>
    `;
    return;
  }

  const html = data.map(r => {
    const image = r.profileImage 
      ? `https://localhost:7039/${r.profileImage}` 
      : "https://via.placeholder.com/300x200?text=Sin+Imagen";

    return `
    <div class="col-md-6 col-lg-4 mt-4 me-4">
      <div class="card border-0 shadow-sm h-100 rounded-4 overflow-hidden">

  
        <div style="height: 200px; overflow: hidden;">
          <img src="${image}" 
               class="w-100 h-100" 
               style="object-fit: cover; transition: transform .3s;">
        </div>


        <div class="bg-light px-3 py-2 border-bottom">
          <h6 class="fw-bold text-primary mb-0">
            <i class="bi bi-person-fill"></i>
            ${r.name ?? "Sin nombre"} ${r.lastName ?? ""}
          </h6>
        </div>

   
        <div class="card-body d-flex flex-column">

          <p class="mb-2 text-dark">
            <i class="bi bi-envelope-fill text-success me-1"></i>
            <span class="fw-semibold">${r.email ?? "Sin email"}</span>
          </p>

          <p class="mb-2 text-muted small">
            <i class="bi bi-person-badge-fill text-warning me-1"></i>
            Usuario: <span class="fw-semibold text-dark">${r.userName ?? "-"}</span>
          </p>

          <span class="badge bg-dark text-warning mb-3 align-self-start">
            ${r.phone ?? "Sin teléfono"}
          </span>

       
          <div class="mt-auto d-flex justify-content-between">

            <a href="/Assets/view/Admin/usuario/edit.html?id=${r.id}"
               class="btn btn-outline-warning btn-sm px-3 rounded-pill">
              <i class="bi bi-pencil"></i> Edit
            </a>

            <a href="/Assets/view/Admin/usuario/delete.html?id=${r.id}" 
               class="btn btn-outline-danger btn-sm px-3 rounded-pill">
              <i class="bi bi-trash"></i> Delete
            </a>

          </div>
        </div>
      </div>
    </div>
    `;
  }).join("");

  containerUsuarios.innerHTML = html;
}


document.addEventListener("DOMContentLoaded", () => {
    getUsers();
});








