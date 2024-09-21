import { currencyFormatter } from "./Utils";

interface AmountProps {
    amount: number
}

function Amount({ amount }: AmountProps) {
    return (
        <span className={`whitespace-nowrap ${amount < 0 ? 'text-red-500' : ''}`}>{currencyFormatter.format(amount)}</span>
    )
}

export default Amount;
