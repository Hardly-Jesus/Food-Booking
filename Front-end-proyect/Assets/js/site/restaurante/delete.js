
const params = new URLSearchParams(window.location.search);
const id = params.get("id");

const btnEliminar = document.getElementById("btnEliminar");
const btnCancelar = document.getElementById("btnCancelar");





btnEliminar.addEventListener("click", async () => {
    try {

        const token = localStorage.getItem("token");

        await axios.delete(
            `${config.API_URL}/Api/v1/Restaurante/Delete-restaurante/${id}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );

     
        window.location.href = "/Assets/view/Propietario/restaurante/Restaurantes.html";

    } catch (err) {
        console.log("Error al eliminar", err);
    }
});