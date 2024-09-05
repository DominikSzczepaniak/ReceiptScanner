export function parseDate(date: Date){
    const _date = new Date(date);
    const day = _date.getDate();
    const month = _date.getMonth();
    const year = _date.getFullYear();
    return `${year}-${month}-${day}`;
}