const params = new URLSearchParams(window.location.search);
const id = params.get("id");

const btnEliminar = document.getElementById("btnEliminar");



btnEliminar.addEventListener("click", async () => {
    try {

        const token = localStorage.getItem("token");

        await axios.delete(
            `${config.API_URL}/Api/v1/Pedido/delete-pedido/${id}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );

     
        window.location.href = "/Assets/view/cliente/ordenes/Ordenes.html";

    } catch (err) 
    {
        console.log("Error al eliminar la notificacion", err);
    }
});

