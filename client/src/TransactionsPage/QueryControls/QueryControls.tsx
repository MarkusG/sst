import { QueryParameters } from "../../QueryParameters";
import DateRangeForm, { DateRangeValues } from "./DateRangeForm";

export interface QueryControlsProps {
    params: QueryParameters,
    onParamsUpdated: (params: QueryParameters) => Promise<any>
}

export default function QueryControls({ params, onParamsUpdated }: QueryControlsProps) {
    async function change(e: React.ChangeEvent<HTMLSelectElement>) {
        const value = e.target.value;
        await onParamsUpdated(new QueryParameters({ ...params, pageSize: Number(value) }));
    }

    async function dateRangeChanged(values: DateRangeValues) {
        await onParamsUpdated(new QueryParameters({ ...params, from: values.from, to: values.to }));
    }

    return (
        <div className="ml-2 mb-1">
            <div className="mb-1">
                <label htmlFor="pageSize" className="mr-1">Page Size:</label>
                <select id="pageSize" className="w-14 text-center border rounded"
                    value={params.pageSize}
                    onChange={change}>
                    <option>100</option>
                    <option>50</option>
                    <option>40</option>
                    <option>30</option>
                </select>
            </div>
            <div>
                <DateRangeForm onSubmit={dateRangeChanged}/>
            </div>
        </div>
    );
}
