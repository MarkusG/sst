import { Field, Form, Formik } from "formik";
import * as yup from "yup";

interface RawDateRangeValues {
    from?: string,
    to?: string,
}

export interface DateRangeValues {
    from?: Date,
    to?: Date,
}

export interface DateRangeFormProps {
    onSubmit: (values: DateRangeValues) => Promise<any>
}

export default function DateRangeForm({ onSubmit }: DateRangeFormProps) {
    const initialValues: RawDateRangeValues = { from: '', to: '' };
    const schema = new yup.ObjectSchema<RawDateRangeValues>({
        from: yup.string()
            .test({
                test: (v, ctx) => !v || !ctx.parent.to || new Date(v) <= new Date(ctx.parent.to)
            }),
        to: yup.string()
    });

    async function submit(rawValues: RawDateRangeValues) {
        const values: DateRangeValues = {
            from: rawValues.from ? new Date(rawValues.from) : undefined,
            to: rawValues.to ? new Date(rawValues.to) : undefined
        };
        await onSubmit(values);
    }

    return (
        <Formik
            initialValues={initialValues}
            validationSchema={schema}
            onSubmit={submit}>
            {props =>
                <Form>
                    <label htmlFor="from" className="mr-1">From:</label>
                    <Field id="from" name="from" type="date" className={`w-36 text-center border ${(props.errors.from && props.touched.from) ? 'border-red-500' : ''} rounded`}/>
                    <label htmlFor="to" className="mx-1">To:</label>
                    <Field id="to" name="to" type="date" className={`w-36 text-center border ${(props.errors.from && props.touched.from) ? 'border-red-500' : ''} rounded`}/>
                    <button type="submit" className="ml-2 text-gray-700"><i className="fa fa-arrow-right"></i></button>
                </Form>
            }
        </Formik>
    );
}
