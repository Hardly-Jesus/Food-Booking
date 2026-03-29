const container = document.getElementById("notificacionesContainer");




async function getNotificaciones(){

    try{

        const token = localStorage.getItem("token");
        const result = await axios.get("https://localhost:7039/Api/v1/Notificacion/get-notificaciones-usuario",{
            headers: {
               
                Authorization: `Bearer ${token}`
                
            }
        });
          
    
        renderNotificaciones(result.data);
    
    }catch(err){

        console.log("Error al obtener restaurante", err);
    }
}


function renderNotificaciones(data) {

  container.innerHTML = "";

  const userId = localStorage.getItem("UsuarioId");

  data.forEach(n => {
      const esSender = n.senderId === userId;

    const card = `
      <div class="col-md-6 col-lg-4 mt-4">
        <div class="card shadow-sm rounded-4 ${esSender ? "border-primary" : "border-success"}">

          <div class="card-body d-flex flex-column">

            <h6 class="fw-bold mb-2">
              <i class="bi ${esSender ? "bi-send" : "bi-bell"}"></i> 
              ${esSender ? "Enviada" : "Recibida"}
            </h6>

            <p class="text-muted small mb-2">
              ${n.descripcion ?? "Sin descripción"}
            </p>

            <span class="badge bg-dark mb-2">
              ${n.tipo ?? "General"}
            </span>

            <small class="text-secondary mb-3">
              ${new Date(n.fecha).toLocaleString()}
            </small>

          
        <div class="mt-auto text-end">
          <a href="/Assets/view/Propietario/notificacion/delete.html?id=${n.id}" class="text-decoration-none"> 
          <button class="btn btn-outline-danger btn-sm px-3">
            <i class="bi bi-trash"></i>
          </button>
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
    getNotificaciones();
});
































